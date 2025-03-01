using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Telegram.Bot;
using Weather_bot.Controllers;

namespace Weather_bot.Commands
{
    public class GeneralCommand
    {
        protected ITelegramBotClient _bot { get; set; }
        protected CancellationToken _cancellationToken { get; set; }
        private StartCommands _startCommands;
        
        public GeneralCommand(ITelegramBotClient bot, CancellationToken cancellationToken) 
        {
            _bot = bot;
            _cancellationToken = cancellationToken;
            _startCommands = new StartCommands(_bot, cancellationToken);
        }
        public async Task ExecuteStartAsync(long chatId, string name) 
        {
            await _startCommands.ExecuteAsync(chatId, name);
        }
    }
}
