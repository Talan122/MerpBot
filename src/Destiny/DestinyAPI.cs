using MerpBot.Destiny.ResponseTypes.User;
using MerpBot.Interactive.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny
{
    public class DestinyAPI
    {
        public static HttpClient HttpClient = new HttpClient();
        public static readonly string Token = "3ff2da6ce9e94c21ae34b7657e1ad7f7";
        public static readonly string RootPath = "https://www.bungie.net/Platform";

        public static App App { get; set; } = new App();

        public static async Task<string> Get(string Endpoint)
        {
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("X-API-Key", "3ff2da6ce9e94c21ae34b7657e1ad7f7");
            return await HttpClient.GetStringAsync($"{RootPath}/{Endpoint}");
        }

        public static async Task<string> Post(string Endpoint, string Content)
        {
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("X-API-Key", "3ff2da6ce9e94c21ae34b7657e1ad7f7");

            HttpContent httpContent = new StringContent(Content);

            return await (await HttpClient.PostAsync($"{RootPath}/{Endpoint}", httpContent)).Content.ReadAsStringAsync();
        }
    }

}
