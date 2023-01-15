using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Interactive.JsonObjects
{
    public class DogObj
    {
        public string message { get; set; }
        public string status { get; set; }
    }

    public class FoxObj
    {
        public string image { get; set; }
        public string link { get; set; }
    }

    public class DuckObj
    {
        public string message { get; set; }
        public string url { get; set; }
    }

    public class QuoteObj
    {
        public string _id { get; set; }
        public string[] tags { get; set; }
        public string content { get; set; }
        public string author { get; set; }
        public int length { get; set; }
        public string dateAdded { get; set; }
        public string dateModified { get; set; }
    }

    public class AxolotlObj
    {
        public string url { get; set; }
        public string facts { get; set; }
        public string pics_repo { get; set; }
        public string api_repo { get; set; }
    }

    public class CatFactObj
    {
        public string[] data { get; set; }
    }
}
