using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Core.Abstractions;
using DataAccess;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Vpn_Telegram
{
    public static class Injection
    {
        public static IServiceProvider GetServiceProvider() 
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMemoryCache();
            serviceCollection.AddDbContext<TelegramDbContext>(options => options.UseNpgsql(appsettingJsonReader.GetConnectionString()));
            serviceCollection.AddScoped<ITelegramUserRepository, TelegramUserRepository>();
            serviceCollection.AddScoped<ITelegramUserService, TelegramUserService>();
            serviceCollection.AddScoped<BotHandler>();
            serviceCollection.AddHttpClient();
            serviceCollection.AddScoped<GetWeatherService>();
            serviceCollection.AddScoped<GetLatLonData>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
