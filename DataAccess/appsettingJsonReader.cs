using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataAccess
{
    public static class appsettingJsonReader
    {
        private readonly static string _path = "C:\\Users\\jazer\\source\\repos\\Vpn_Telegram\\DataAccess\\appsettings.json";
        public static string GetConnectionString() { 
            string value;
            using (var reader = new StreamReader(_path))
            {
                value = reader.ReadToEnd();
                var json = JsonConvert.DeserializeObject<ConString>(value) ?? throw new Exception("I cant find your path");
                
                return json.ConnectionStrings;
                
            }
        }
    }
    internal record ConString(string ConnectionStrings);
}
