using Newtonsoft.Json;
using Telegram.Bot;

namespace Infrastructure
{
    public static class TelegramBotClientFabric
    {
        private readonly static string _path = "C:\\Users\\jazer\\source\\repos\\Vpn_Telegram\\secret.json";
        public async static Task<TelegramBotClient> GetTelegramBotClientAsync(CancellationToken token)
        {
            
            SecretKey secretKey;
            using (StreamReader reader = new StreamReader(_path))
            {
                string fullJson = await reader.ReadToEndAsync();
                secretKey = JsonConvert.DeserializeObject<SecretKey>(fullJson) ?? throw new Exception("I cant find this token");

            }
            return new TelegramBotClient(secretKey.Key, cancellationToken: token);
        }
    }
    public record SecretKey(string Key);
}
