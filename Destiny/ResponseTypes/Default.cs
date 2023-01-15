using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny.ResponseTypes
{
    public class Default
    {
        public Int32? ErrorCode { get; set; }
        public Int32? ThrottleSeconds { get; set; }
        public string? ErrorStatus { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, string>? MessageData { get; set; }
        public string? DetailedErrorTrace { get; set; }
    }
}
