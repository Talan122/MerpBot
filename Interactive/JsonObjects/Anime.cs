using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Interactive.JsonObjects
{
    // Currently useless, the API was down when I made this. It may come back at some point.
    public class AnimeFacts
    {
        public class List
        {
            public class Data
            {
                public int anime_id { get; set; }
                public string anime_name { get; set; }
                public string anime_img { get; set; }
            }
            public bool success { get; set; }
            public Data[] data { get; set; }

            public string CombineNames()
            {
                string result = "";

                foreach (var item in data)
                {
                    result += $"{item.anime_name}, ";
                }

                result = result.Substring(0, result.Length - 2);
                return result;
            }
        }
    }

    public class AnimeQuotes
    {
        public string anime { get; set; }
        public string character { get; set; }
        public string quote { get; set; }
    }
}
