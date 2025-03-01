using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application;
using Core.Abstractions;
using Core.Enums;
using Core.Exceptions;
using Infrastructure;
using Microsoft.AspNetCore.Http;
using Telegram.Bot;
using Telegram.Bot.Types;
using Weather_bot.Commands;
using Weather_bot.Commands.Keyboard;

namespace Weather_bot.Actions
{
    public class ActionByKey
    {
        private readonly IRedisService _redisService;
        private readonly GetWeatherService _weatherService;
        private readonly ITelegramUserService _telegramUserService;
        public ActionByKey(IRedisService redisService, GetWeatherService weatherService, ITelegramUserService telegramUserService)
        {
            _redisService = redisService;
            _weatherService = weatherService;
            _telegramUserService = telegramUserService;
        }
        public async Task DoAction(ITelegramBotClient bot, Message sender, CancellationToken cancellationToken) 
        {
            try
            {
                if (sender.From == null) throw new Exception("неизвестный Id");
                var message = sender.Text ?? throw new IncorrectMessageFormatException("Неверный формат сообщения"); 
                var key = sender.From.Id.ToString(); 
                var value = await _redisService.Db.StringGetAsync(key);
                if (value.IsNullOrEmpty == false)
                {
                    switch (Enum.Parse<StatesOfUser>(value.ToString()))
                    {

                        case StatesOfUser.WaitingWeather:
                            
                            await WeatherCommands.PostWeather(bot, long.Parse(key), message, _weatherService, cancellationToken);
                            break;
                        case StatesOfUser.WaitingName:
                            await NameCommands.ChangeName(bot, sender, _telegramUserService, cancellationToken);
                            break;


                        default:
                                throw new Exception("Неизвестное состояние пользователя");
                    }
                    await _redisService.Db.KeyDeleteAsync(key);
                }
            } catch (BadHttpRequestException ex)
            {
                Console.WriteLine("не удалось выполнить запрос");
                if (sender.From != null)
                {
                    await bot.SendMessage(sender.From.Id, $"не удалось выполнить запрос(", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (sender.From != null)
                {
                    await bot.SendMessage(sender.From.Id, $"{ex.Message}", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
                }
            }
            finally 
            {
                if (sender.From != null)
                {
                    await _redisService.Db.KeyDeleteAsync(sender.From.Id.ToString());
                }
            }
            
        }
    }
}
