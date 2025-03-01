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
            string cityCorrect = string.Empty;
            for (int i = 0; i < message.Length; i++)
            {
                if (i == 0 || (message.IndexOf(' ') != -1 && i == (message.IndexOf(' ') + 1)) || (message.IndexOf('-') != -1 && i == (message.IndexOf('-') + 1)))
                {
                    cityCorrect += message[i].ToString().ToUpper();
                    continue;
                }
                
                    cityCorrect += message[i];

            }
            if (cityCorrect.Contains(" "))
            {
                cityCorrect.Replace(' ', '_');
            }
            return cityCorrect;
        }
    }
}
