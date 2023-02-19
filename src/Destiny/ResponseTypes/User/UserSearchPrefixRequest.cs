using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny.ResponseTypes.User
{
    public class UserSearchPrefixRequest
    {
        public string displayNamePrefix { get; set; }
        public UserSearchPrefixRequest(string displayNamePrefix)
        {
            this.displayNamePrefix = displayNamePrefix;
        }
        public string ToJson()
        {
            return $"{{\"displayNamePrefix\":\"{displayNamePrefix}\"}}";
        }
    }
}
