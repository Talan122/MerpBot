using System;
using MerpBot.Destiny.ResponseTypes.Ignores;

namespace MerpBot.Destiny.ResponseTypes.User
{
    public class UserToUserContext
    {
        public bool isFollowing { get; set; }
        public IgnoreResponse ignoreStatus { get; set; }
        public DateTime globalIgnoreEndDate { get; set; }
    }
}
