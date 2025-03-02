using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using StackExchange.Redis;
using Telegram.Bot.Types.ReplyMarkups;
using Core.Enums;
using Infrastructure;
using System.Threading;
using Telegram.Bot.Types;
using Vpn_Telegram;
using Microsoft.Extensions.DependencyInjection;
using Weather_bot.Commands.Keyboard;

namespace Weather_bot.Commands
{
    public static class WeatherCommands
    {
        public static async Task WaitCityAsync(IDatabase db, ITelegramBotClient bot, long chatId, string username, CancellationToken cancellationToken)
        {
            await db.StringSetAsync(chatId.ToString(), StatesOfUser.WaitingWeather.ToString());
            await bot.SendMessage(chatId, $"{username}, Введите название города", replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
        }
        public static async Task PostWeather(ITelegramBotClient bot, long chatId, string message, GetWeatherService weatherService, CancellationToken cancellationToken)
        {
            string city = GetCorrectCityName.Get(message.Trim());
            var weather = await weatherService.GetWeather(city, cancellationToken);
            await bot.SendMessage(chatId, $"В населенном пункте {city} такая погода:\n" +
                $"Температура: {weather.fact.temp}°C\n" +
                $"Ощущаемая температура: {weather.fact.feels_like}°C\n" +
                $"Температура воды: {weather.fact.temp_water}°C\n" +
                $"Температура воды: {weather.fact.temp_water}°C\n" +
                $"Погодное описание: {weather.fact.condition}\n" +
                $"Скорость ветра: {weather.fact.wind_speed}\n" +
                $"Направление ветра: {weather.fact.wind_dir}\n" +
                $"Влажность воздуха: {weather.fact.humidity}%\n" +
                $"Светлое или темное время суток: {weather.fact.daytime}\n" +
                $"Время года: {weather.fact.season}\n" +
            $"Есть ли гроза: {weather.fact.is_thunder}\n" +
                $"Облачность: {weather.fact.cloudness}\n", replyMarkup: KeyboardService.GetMainKeyboard(), cancellationToken: cancellationToken);
            for (int i = 0; i < weather.fact.imageUrl?.Count; i++) 
            {
                await bot.SendPhoto(chatId, photo: InputFile.FromString(weather.fact.imageUrl[i]), caption: $"Изображение города номер {i + 1}", cancellationToken: cancellationToken);
  
            }
            Console.WriteLine($"{city}    {city.Length}");
        }
    }
}
