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
    public class StartCommands : GeneralCommand
    {
        public StartCommands(ITelegramBotClient bot, CancellationToken cancellationToken) : base(bot, cancellationToken) {}
        public async Task ExecuteAsync(long chatId, string username)
        {
            await _bot.SendMessage(chatId, $"Привет, {username} я твой тг бот для погоды !", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: _cancellationToken);
        }
    }
}
