using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny.ResponseTypes.User
{
    public class UserSearchResponseDetail
    {
        public string bungieGlobalDisplayName { get; set; }
        public Int16? bungieGlobalDisplayNameCode { get; set; }
        public Int64? bungieNetMembershipId { get; set; }
        public UserInfoCard[] destinyMemberships { get; set; }
    }
}
