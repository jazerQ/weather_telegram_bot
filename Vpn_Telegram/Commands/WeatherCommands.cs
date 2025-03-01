using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using StackExchange.Redis;
using Telegram.Bot.Types.ReplyMarkups;

namespace Weather_bot.Commands
{
    public static class WeatherCommands
    {
        public static async Task WaitCityAsync(IDatabase db, ITelegramBotClient bot, long chatId, string username, CancellationToken cancellationToken)
        {
            await db.StringSetAsync("waiting-for-city", chatId);
            await bot.SendMessage(chatId, "Введите название города", replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
            
        }
    }
}
