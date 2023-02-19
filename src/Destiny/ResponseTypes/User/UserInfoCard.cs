using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny.ResponseTypes.User
{
    public class UserInfoCard
    {
        /// <summary>
        /// A platform specific additional display name - ex: psn Real Name, bnet Unique Name, etc.
        /// </summary>
        public string supplementalDisplayName { get; set; }
        /// <summary>
        /// URL the Icon if available.
        /// </summary>
        public string iconPath { get; set; }
        /// <summary>
        /// If there is a cross save override in effect, this value will tell you the type that is overridding this one.
        /// </summary>
        public Int32 crossSaveOverride { get; set; }
        /// <summary>
        /// The list of Membership Types indicating the platforms on which this Membership can be used.
        ///
        /// Not in Cross Save = its original membership type.Cross Save Primary = Any membership types it is overridding, and its original membership type Cross Save Overridden = Empty list
        /// </summary>
        public Int32[] applicableMembershipTypes { get; set; }
        /// <summary>
        /// If True, this is a public user membership.
        /// </summary>
        public bool isPublic { get; set; }
        /// <summary>
        /// Type of the membership. Not necessarily the native type.
        /// </summary>
        public Int32 membershipType { get; set; }
        /// <summary>
        /// Membership ID as they user is known in the Accounts service
        /// </summary>
        public Int64 membershipId { get; set; }
        /// <summary>
        /// Display Name the player has chosen for themselves. The display name is optional when the data type is used as input to a platform API.
        /// </summary>
        public string displayName { get; set; }
        /// <summary>
        /// The bungie global display name, if set.
        /// </summary>
        public string bungieGlobalDisplayName { get; set; }
        /// <summary>
        /// The bungie global display name code, if set.
        /// </summary>
        public Int16? bungieGlobalDisplayNameCode { get; set; }
    }
}
