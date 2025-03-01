
using DataAccess;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Vpn_Telegram;
using StackExchange.Redis;

internal class Program
{
    private static IServiceProvider _service = Injection.GetServiceProvider();
    private static TelegramBotClient bot;
    //private static FromMemoryCacheToDb _toDb = service.GetService<FromMemoryCacheToDb>();
    private async static Task Main(string[] args)
    {
        var scope = _service.CreateScope();
        var botHandler = scope.ServiceProvider.GetRequiredService<BotHandler>();
        using var cts = new CancellationTokenSource();
        bot = await TelegramBotClientFabric.GetTelegramBotClientAsync(cts.Token);
        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = new[] { UpdateType.Message },
            
        };
        bot.StartReceiving(botHandler.HandleUpdateAsync, botHandler.HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);
        var me = await bot.GetMe();
        Console.WriteLine($"Бот под названием {me.FirstName} запущен");
        Console.ReadLine();
        cts.Cancel();
    }
    
    
}