using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny.ResponseTypes.User
{
    public class UserSearchResponse
    {
        public UserSearchResponseDetail[] searchResults { get; set; }
        public Int32 page { get; set; }
        public bool hasMore { get; set; }
    }
}
