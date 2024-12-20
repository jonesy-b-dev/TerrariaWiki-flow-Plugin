using System.Collections.Generic;
using Flow.Launcher.Plugin;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using Flow.Launcher.Plugin.SharedCommands;
using Newtonsoft.Json.Linq;

namespace Flow.Launcher.Plugin.TerrariaWiki
{
    public class TerrariaWiki : IAsyncPlugin, ISettingProvider, IContextMenu
    {
        private PluginInitContext _context;
        private Settings _settings;

        // Define variables for the plugin to use
        static private string fandomUrl = "https://terraria.fandom.com/";
        static private string wikiggUrl = "https://terraria.wiki.gg/";
        private string BaseUrl => _settings.useFandom ? fandomUrl : wikiggUrl;
        private string QueryUrl => BaseUrl + "api.php?action=query&list=search&srwhat=text&format=json&srsearch=";
        private string jsonResult;
        private string finalUrl;

        // Path to icon
        private readonly string icon_path = "icon.png";

        // Initialise query url
        public async Task InitAsync(PluginInitContext context)
        {
            _context = context;
            _settings = _context.API.LoadSettingJsonStorage<Settings>();
        }

        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            // Make request to terraria wiki api with the query the user has put in
            using (var httpClient = new HttpClient())
            {
                // Set headers to mimic a browser request
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                jsonResult = await httpClient.GetStringAsync(QueryUrl + query.Search);
            }

            // Store the data
            dynamic data = JObject.Parse(jsonResult);
            // Also store as a JObject so we can check if it contains the error key
            JObject dataObj = JObject.Parse(jsonResult);

            // Check if the data has error key (happens when the user has not given an input yet)
            if (dataObj.ContainsKey("error"))
            {
                // Make simple result 
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

                // Loop over all the results and make a result item for them all
                foreach (var item in data.query.search)
                {
                    string itemStr = item.title;
                    string itemWithunderscores = itemStr.Replace(" ", "_");

                    results.Add(new Result
                    {
                        Title = $"{item.title}",
                        SubTitle = BaseUrl + "wiki/" + itemWithunderscores,
                        Action = e =>
                        {
                            // Make final url to search
                            finalUrl = BaseUrl + "wiki/" + itemWithunderscores;
                            finalUrl.OpenInBrowserTab();
                            return true;
                        },
                        IcoPath = "icon.png"
                    });
                }
                // Return the results
                return await Task.FromResult(results);
            }
        }
        public Control CreateSettingPanel() => new SettingsControl(_settings);

        public List<Result> LoadContextMenus(Result selectedResult)
        {
            var results = new List<Result>
            {

            };

            return results;
        }
    }
}
