using MerpBot.Destiny.ResponseTypes;
using MerpBot.Destiny.ResponseTypes.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny
{
    public class GetBungieNetUserById : Default
    {
        public GeneralUser Response { get; set; }
    }

    public class SearchByGlobalNamePost : Default
    {
        public UserSearchResponse Response { get; set; }
    }

    public class User
    {
        public static async Task<GetBungieNetUserById> GetBungieNetUserById(Int64 ID)
        {
            GetBungieNetUserById? json = JsonConvert.DeserializeObject<GetBungieNetUserById>(await DestinyAPI.Get($"User/GetBungieNetUserById/{ID}/"));

            if(json == null) throw new NullReferenceException();

            return json;
        }
        /// <summary>
        /// Given the prefix of a global display name, returns all users who share that name.
        /// </summary>
        /// <param name="Input">The input name</param>
        /// <param name="Page">The zero-based page of results you desire.</param>
        /// <returns>Look at the Response property for more information about the nature of this response</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task<SearchByGlobalNamePost> SearchByGlobalNamePost(UserSearchPrefixRequest Input, Int32 Page)
        {
            SearchByGlobalNamePost? json = JsonConvert.DeserializeObject<SearchByGlobalNamePost>(await DestinyAPI.Post($"User/Search/GlobalName/{Page}/", Input.ToJson()));

            if (json == null) throw new NullReferenceException();

            return json;
        }
    }
}
