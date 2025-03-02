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
        public async Task<List<string>> GetFirstImageUrlFromWiki(string url)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var photos = new List<string>();
            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(html));

            var imageElements = document.QuerySelectorAll("img"); // Берем первую картинку
            if (imageElements != null) {
                foreach (var image in imageElements)
                    if (image != null)
                    {
                        
                        string? src = image.GetAttribute("src");

                        if (src == null) continue;
                        // Добавляем "https:" к относительным ссылкам
                        if (!src.StartsWith("http"))
                        {
                            src = "https:" + src;
                        }

                        photos.Add(src);
                        if (photos.Count == 3) break;
                    }
            }
            return photos;
        }
    }
}
