using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Flow.Launcher.Plugin.SharedCommands;
using System.Net.Http;
using System;

namespace Flow.Launcher.Plugin.TerrariaWiki
{
    public class TerrariaWiki : IAsyncPlugin
    {
        private PluginInitContext _context;

        private readonly string base_url = "https://terraria.fandom.com/";
        private string query_url;
        private string jsonResult;
        private string finalUrl;

        private readonly string icon_path = "icon.png";

        public async Task InitAsync(PluginInitContext context)
        {
            query_url = base_url + "api.php?action=query&list=search&srwhat=text&format=json&srsearch=";

            _context = context;
        }

        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            using(var httpClient = new HttpClient())
            {
                jsonResult = await httpClient.GetStringAsync(query_url + query.Search);
            }

            dynamic data = JObject.Parse(jsonResult);
            JObject dataObj = JObject.Parse(jsonResult);

            if (dataObj.ContainsKey("error"))
            {
                var noResults = new List<Result>
                {
                    new Result
                    {
                        Title = $"Start typing to search the wiki...",
                        IcoPath = "icon.png"
                    }
                };
                return await Task.FromResult(noResults);
            }
            else
            {
                var results = new List<Result>();
                
                foreach (var item in data.query.search)
                {
                    results.Add(new Result
                    {
                        Title = $"{item.title}",
                        SubTitle = $"https://terraria.fandom.com/wiki/" + item.title,
                        Action = e =>
                        {
                            finalUrl = base_url + "wiki/" + item.title;
                            finalUrl.OpenInBrowserTab();
                            return true;
                        },
                        IcoPath = "icon.png"
                    });
                }

                return await Task.FromResult(results);
            }
        }
    }
}