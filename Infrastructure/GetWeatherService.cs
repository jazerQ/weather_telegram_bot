using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Infrastructure
{
    public class GetWeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GetFirstImageFromWiki _imageFromWiki;
        private readonly GetLatLonData _latLonData;
        public GetWeatherService(IHttpClientFactory httpClientFactory, GetLatLonData latLonData, GetFirstImageFromWiki imageFromWiki)
        {
            _httpClientFactory = httpClientFactory;
            _latLonData = latLonData;
            _imageFromWiki = imageFromWiki;
        }                                 //lat - широта    lon - долгота
        public async Task<WeatherResponse> GetWeather(string city, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient();
            (string, string) latlon = await _latLonData.Get(city);
            
            httpClient.DefaultRequestHeaders.Add("X-Yandex-Weather-Key", Environment.GetEnvironmentVariable("YandexToken"));
            //httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, gzip");
            using HttpResponseMessage response = await httpClient.GetAsync($"https://api.weather.yandex.ru/v2/forecast?lat={latlon.Item1}&lon={latlon.Item2}", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync(cancellationToken);
                var weather = JsonConvert.DeserializeObject<WeatherResponse>(content);
                if (weather != null) 
                {
                    weather.fact.imageUrl = await _imageFromWiki.GetFirstImageUrlFromWiki($"https://ru.wikipedia.org/wiki/{city}"); 
                    return weather;
                }
                throw new WeatherIsNullException("не удалось найти погоду");
            }
            throw new BadHttpRequestException("не получилось обратиться в сторонний сервис");



        }
    }
}
