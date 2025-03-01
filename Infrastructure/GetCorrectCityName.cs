using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Infrastructure
{
    public static class GetCorrectCityName
    {
        public static string Get(string message) 
        {
            string city = message.Remove(0, "/weather".Length + 1).ToLowerInvariant();
            string cityCorrect = string.Empty;
            for (int i = 0; i < city.Length; i++)
            {
                if (i == 0 || (city.IndexOf(' ') != -1 && i == (city.IndexOf(' ') + 1)) || (city.IndexOf('-') != -1 && i == (city.IndexOf('-') + 1)))
                {
                    cityCorrect += city[i].ToString().ToUpper();
                    continue;
                }
                
                    cityCorrect += city[i];

            }
            if (cityCorrect.Contains(" "))
            {
                cityCorrect.Replace(' ', '_');
            }
            return cityCorrect;
        }
    }
}
