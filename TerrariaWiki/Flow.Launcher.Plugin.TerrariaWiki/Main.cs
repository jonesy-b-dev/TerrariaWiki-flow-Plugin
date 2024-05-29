using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Flow.Launcher.Plugin.SharedCommands;
using Newtonsoft.Json.Linq;

namespace Flow.Launcher.Plugin.TerrariaWiki
{
    public class TerrariaWiki : IAsyncPlugin
    {
        private PluginInitContext _context;

        // Define variabkes for the plugin to use
        static private string fandomUrl = "https://terraria.fandom.com";
        static private string wikiggUrl = "https://terraria.wiki.gg/";
        static bool useFandom = false;
        private string base_url;
        private string query_url;
        private string jsonResult;
        private string finalUrl;

        // Path to icon
        private readonly string icon_path = "icon.png";

        // Initialise query url
        public async Task InitAsync(PluginInitContext context)
        {
            base_url = useFandom ? fandomUrl : wikiggUrl;

            query_url = base_url + "api.php?action=query&list=search&srwhat=text&format=json&srsearch=";

            _context = context;
        }

        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            // Make request to terraria wiki api with the query the user has put in
            using (var httpClient = new HttpClient())
            {
                jsonResult = await httpClient.GetStringAsync(query_url + query.Search);
            }

            // Store the daya
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
                        SubTitle = base_url + "wiki/" + itemWithunderscores,
                        Action = e =>
                        {
                            // Make final url to search
                            finalUrl = base_url + "wiki/" + itemWithunderscores;
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
    }
}