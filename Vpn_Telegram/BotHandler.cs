using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Core.Abstractions;
using Core.Entities;
using Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Telegram.Bot;
using Telegram.Bot.Types;
using Weather_bot.Actions;
using Weather_bot.Commands;
using Weather_bot.Commands.Keyboard;
using Weather_bot.Controllers;

namespace Vpn_Telegram
{
    public class BotHandler
    {
        private readonly ITelegramUserService _serviceUser;
        private readonly IRedisService _redisService;
        private readonly GetWeatherService _weatherService;
        private readonly ActionByKey _actionByKey;
        public BotHandler(ITelegramUserService serviceUser, GetWeatherService weatherService, IRedisService redisService, ActionByKey actionByKey)
        {
            _redisService = redisService;
            _weatherService = weatherService;
            _serviceUser = serviceUser;
            _actionByKey = actionByKey;
            
        }
        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message) return;
            if (message.Text is not { } messageText) return;
            string user = await _serviceUser.GetNameById(message.From.Id, cancellationToken) ?? message.From.FirstName;
            long chatId = message.Chat.Id;
            if(await _redisService.Db.KeyExistsAsync(chatId.ToString()))
            {
                await _actionByKey.DoAction(bot, message, cancellationToken);
                return;
            }
            switch (message.Text.ToLower())
            {
                case "/start":
                    await StartCommands.ExecuteAsync(bot, chatId, user, cancellationToken);
                    break;
                case "/help":
                    await bot.SendMessage(chatId, $"список команд для бота: /start - запускает бота \n/help - список возможных команд", cancellationToken: cancellationToken);
                    break;
                case "/setname" when message.Text.Split(' ').Length > 1:
                    
                case "узнать погоду":
                    await WeatherCommands.WaitCityAsync(_redisService.Db, bot, chatId, user, cancellationToken);
                    break;
                case "поменять имя":
                    await NameCommands.ChangeNameRequest(_redisService.Db, bot, chatId, cancellationToken);
                    break;
                case "мое имя":
                    await NameCommands.GetMyName(bot, chatId, user, cancellationToken);
                    break;
                    
                default:
                    await bot.SendMessage(chatId, $"я не понимаю такой команды", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
                    break;

            }
        }
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return;
        }
    }
}
