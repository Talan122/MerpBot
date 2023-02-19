using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Destiny.ResponseTypes;
using MerpBot.Destiny.ResponseTypes.Applications;
using Newtonsoft.Json;

namespace MerpBot.Destiny
{
    public class GetBungieApplications : Default 
    {
        public Application[] Response { get; set; }
    }

    public class App
    {
        public static async Task<GetBungieApplications> FirstParty()
        {
            var json = JsonConvert.DeserializeObject<GetBungieApplications>(await DestinyAPI.Get("App/FirstParty/"));

            if (json == null) throw new NullReferenceException();

            return json;
        }
    }
}
