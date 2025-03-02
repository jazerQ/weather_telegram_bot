using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Weather_bot.Commands.Keyboard;

namespace Weather_bot.Commands
{
    public static class AboutUsCommands
    {
        public static async Task GetInfo(ITelegramBotClient bot, long chatId, string username, CancellationToken cancellationToken)
        {
            await bot.SendMessage(chatId, $"{username}, вот ссылки, если ты зайдешь на Github, то поставь звезду❤️", replyMarkup: KeyboardService.GetInlineKeyboardAboutProject(), cancellationToken: cancellationToken);
        }
    }
}
