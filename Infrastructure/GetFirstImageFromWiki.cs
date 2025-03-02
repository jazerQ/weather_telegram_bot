using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class GetFirstImageFromWiki
    {
        public async Task<string> GetFirstImageUrlFromWiki(string url)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(html));

            var imageElement = document.QuerySelector("img"); // Берем первую картинку

            if (imageElement != null)
            {
                string src = imageElement.GetAttribute("src");

                // Добавляем "https:" к относительным ссылкам
                if (!src.StartsWith("http"))
                {
                    src = "https:" + src;
                }

                return src;
            }

            return "Картинка не найдена";
        }
    }
}
