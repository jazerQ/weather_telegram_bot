using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Weather_bot.Commands;
using Weather_bot.Commands.Keyboard;

namespace Weather_bot.Controllers
{
    public static class StartCommands
    {
        public static async Task ExecuteAsync(ITelegramBotClient bot, long chatId, string username, CancellationToken cancellationToken)
        {
            await bot.SendMessage(chatId, $"Привет, {username} я твой тг бот для погоды !", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
        }
    }
}
