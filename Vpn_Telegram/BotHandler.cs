using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Entities;
using Infrastructure;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Vpn_Telegram
{
    public class BotHandler
    {
        private readonly ITelegramUserService _serviceUser;
        private readonly GetWeatherService _weatherService;
        public BotHandler(ITelegramUserService serviceUser, GetWeatherService weatherService)
        {
            _weatherService = weatherService;
            _serviceUser = serviceUser;
        }
        public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message) return;
            if (message.Text is not { } messageText) return;
            long chatId = message.Chat.Id;
            switch (message.Text.Split(' ')[0].ToLower())
            {
                case "/start":
                    await bot.SendMessage(chatId, $"Id - {message.From.Id}\nFirstname - {message.From.FirstName}, LastName - {message.From.LastName},Username - {message.From.Username} , Привет я твой телеграмм бот для VPN 🚀!", cancellationToken: cancellationToken);
                    break;
                case "/help":
                    await bot.SendMessage(chatId, $"список команд для бота: /start - запускает бота \n/help - список возможных команд", cancellationToken: cancellationToken);
                    break;
                case "/setname" when message.Text.Split(' ').Length > 1:
                    try
                    {
                        var nickname = message.Text.Split(' ')[1];
                        if (nickname.Length > 50) throw new Exception("Too big name");
                        if (nickname.Length < 5) throw new Exception("Very small name");
                        TelegramUser tgUser = new TelegramUser
                        {
                            Id = message.From.Id,
                            FirstName = message.From.FirstName,
                            LastName = message.From.LastName ?? "NaN",
                            Name = nickname,
                            Shortname = message.From.Username ?? "NaN",
                            StartDate = DateTime.UtcNow
                        };
                        await _serviceUser.AddUser(tgUser, cancellationToken);
                        await bot.SendMessage(chatId, $"ооо привет, {nickname}!", cancellationToken: cancellationToken);
                        break;
                    }
                    catch (Exception ex)
                    {
                        await bot.SendMessage(chatId, $"имя не подходит по длине", cancellationToken: cancellationToken);
                        break;
                    }
                case "/weather" when message.Text.Split(' ').Length > 1:
                    string city = GetCorrectCityName.Get(message.Text.Trim());
                    var weather = await _weatherService.GetWeather(city, cancellationToken);
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
                        $"Облачность: {weather.fact.cloudness}\n", cancellationToken: cancellationToken);
                    Console.WriteLine($"{city}    {city.Length}");
                    break;
                case "/weather":
                    await bot.SendMessage(chatId, "пропиши /weather {город} чтобы получить данные", cancellationToken: cancellationToken);
                    break;
                case "/setname":
                    await bot.SendMessage(chatId, "укажите команду /setname {имя}", cancellationToken: cancellationToken);
                    break;
                case "/myname":
                    try
                    {
                        var n1 = DateTime.Now;
                        string user = await _serviceUser.GetNameById(message.From.Id, cancellationToken);
                        await bot.SendMessage(chatId, $"привет {user}", cancellationToken: cancellationToken);
                        Console.WriteLine(DateTime.Now - n1);
                        break;
                        
                    }catch(Exception ex)
                    {
                        await bot.SendMessage(chatId, "не могу найти имя, воспольлуйтесь командой /setname {name}", cancellationToken: cancellationToken);
                        break;
                    }
                    
                default:
                    await bot.SendMessage(chatId, $"я не понимаю такой команды", cancellationToken: cancellationToken);
                    break;

            }
        }
        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Ошибка: {exception.Message}");
            return;
        }
    }
}
