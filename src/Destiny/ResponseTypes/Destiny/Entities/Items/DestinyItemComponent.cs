using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Destiny.ResponseTypes.Destiny.Quests;

namespace MerpBot.Destiny.ResponseTypes.Destiny.Entities.Items
{
    public class DestinyItemComponent
    {
        public UInt32 itemHash { get; set; }
        public Int64 itemInstanceId { get; set; }
        public Int32 quantity { get; set; }
        public Int32 bindStatus { get; set; }
        public Int32 location { get; set; }
        public UInt32 bucketHash { get; set; }
        public Int32 transferStatus { get; set; }
        public bool lockable { get; set; }
        public UInt32 overrideStyleItemHash { get; set; }
        public DateTime? expirationDate { get; set; }
        public bool isWrapper { get; set; }
        public Int32[] tooltipNotificationIndexes { get; set; }
        public UInt32 metricHash { get; set; }
        public DestinyObjectiveProgress metricObjective { get; set; }
        public Int32? versionNumber { get; set; }
        public bool[] itemValueVisibility { get; set; }
    }
}
