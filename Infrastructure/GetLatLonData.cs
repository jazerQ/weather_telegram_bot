using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
    public class GetLatLonData
    {
        private readonly IHttpClientFactory _factory;

        public GetLatLonData(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        public async Task<(string, string)> Get(string city)
        {
            try
            {
                var httpClient = _factory.CreateClient();
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://ru.wikipedia.org/wiki/{city}");
                HttpResponseMessage response = await httpClient.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return Parse(content);
                }
                else throw new BadHttpRequestException($"BadRequest {response.StatusCode}");
            }
            catch (BadHttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        private (string, string) Parse(string content)
        {
            try
            {
                string lat = string.Empty;
                string lon = string.Empty;
                var splitedContent = content.Split(' ');
                foreach (var elem in splitedContent)
                {
                    if (elem.Contains("data-lat"))
                    {
                        lat = elem.Split('=')[1];
                    }
                    if (elem.Contains("data-lon"))
                    {
                        lon = elem.Split('=')[1];
                    }
                }
                if (lat == string.Empty || lon == string.Empty) throw new InvalidCityNameException("не нашел город");
                return (lat.Replace('\"', ' ').Trim(), lon.Replace('\"', ' ').Trim());
            }
            catch (InvalidCityNameException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
