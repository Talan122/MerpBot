using MerpBot.Destiny.ResponseTypes;
using MerpBot.Destiny.ResponseTypes.Destiny.Config;
using MerpBot.Destiny.ResponseTypes.Destiny.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny
{
    public class GetDestinyManifest : Default
    {
        public DestinyManifest Response { get; set; }
    }
    public class GetCharacter : Default
    {
        public DestinyCharacterResponse Response { get; set; }
    }

    public class Destiny2
    {
        public static async Task<GetDestinyManifest> GetDestinyManifest()
        {
            GetDestinyManifest? json = JsonConvert.DeserializeObject<GetDestinyManifest>(await DestinyAPI.Get("Destiny2/Manifest/"));

            if (json == null) throw new NullReferenceException();

            return json;
        }
        public static async Task<GetCharacter> GetCharacter()
        {
            GetCharacter? json = JsonConvert.DeserializeObject<GetCharacter>(await DestinyAPI.Get("Destiny2/Manifest/"));

            if (json == null) throw new NullReferenceException();

            return json;
        }
    }
    
}
