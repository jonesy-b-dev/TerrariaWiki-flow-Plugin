using Flow.Launcher.Plugin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Policy;
using Flow.Launcher.Plugin.SharedCommands;
using System.Threading.Tasks;
using System.Threading;

namespace Flow.Launcher.Plugin.TerrariaWiki
{
    public class TerrariaWiki : IAsyncPlugin
    {
        private PluginInitContext _context;

        private readonly string base_url = "https://terraria.fandom.com/";
        private string query_url;

        private readonly string icon_path = "icon.png";

        public async Task InitAsync(PluginInitContext context)
        {
            query_url = base_url + "api.php?action=query&list=search&srwhat=text&format=json&srsearch=";

            _context = context;
        }

        public Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            string finalUrl = base_url + "wiki/" + query.Search;
            
            var results = new List<Result>();

            results.Add(new Result
            {
                Title = "`Search Terraria wiki",
                SubTitle = $" {query_url + query.Search}",
                Action = e =>
                {
                    finalUrl.OpenInBrowserTab();
                    return true;
                },
                IcoPath = "icon.png"
            });
            return Task.FromResult(results);
            //return new Task<List<Result>> { result };
        }
    }
}