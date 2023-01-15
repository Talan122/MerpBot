using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Interactive.Options
{
    public enum AxolotlOptions : int
    {
        image,
        fact
    }

    public class AxolotlHandler
    {
        public static string Parse(AxolotlOptions input)
        {
            if (input == AxolotlOptions.image) return "image";
            if (input != AxolotlOptions.fact) return "fact";
            return "error";
        }
    }
}
