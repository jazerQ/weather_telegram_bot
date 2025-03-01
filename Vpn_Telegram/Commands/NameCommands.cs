using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Core.Entities;
using Core.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using StackExchange.Redis;
using Core.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Weather_bot.Commands.Keyboard;
using Core.Abstractions;

namespace Weather_bot.Commands
{
    public static class NameCommands
    {
        public async static Task GetMyName(ITelegramBotClient bot, long chatId, string username, CancellationToken cancellationToken)
        {
            try
            {
                await bot.SendMessage(chatId, $"привет {username}", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
                

            }
            catch (Exception ex)
            {
                await bot.SendMessage(chatId, "не могу найти имя, воспольлуйтесь командой /setname {name}", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
            }
        }
        public async static Task ChangeNameRequest(IDatabase db, ITelegramBotClient bot, long chatId, CancellationToken cancellationToken)
        {
            await db.StringSetAsync(chatId.ToString(), StatesOfUser.WaitingName.ToString());
            await bot.SendMessage(chatId, "пиши как ты хочешь, чтобы я тебя называл", replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
        }
        public async static Task ChangeName(ITelegramBotClient bot, Message message, ITelegramUserService userService, CancellationToken cancellationToken)
        {
            try
            {
                var name = message.Text;
                if (name.Length > 50) throw new InvalidNameException("слишком большое имя");
                if (name.Length < 5) throw new InvalidNameException("слишком маленькое имя");
                TelegramUser tgUser = new TelegramUser
                {
                    Id = message.From.Id,
                    FirstName = message.From.FirstName,
                    LastName = message.From.LastName ?? "NaN",
                    Name = name,
                    Shortname = message.From.Username ?? "NaN",
                    StartDate = DateTime.UtcNow
                };
                await userService.AddUser(tgUser, cancellationToken);
                await bot.SendMessage(message.From.Id, $"ооо привет, {name}! Я тебя запомнил", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
            }
            catch (InvalidNameException ex) 
            {
                await bot.SendMessage(message.From.Id, $"{ex.Message}", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await bot.SendMessage(message.From.Id, $"ошибка, попробуйте еще раз.", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
            }
        }
    }
}
