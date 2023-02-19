using MerpBot.Destiny.ResponseTypes.Destiny.Entities.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny.ResponseTypes.Destiny.Components
{
    public class SingleComponentResponseOfDestinyInventoryComponent
    {
        public DestinyInventoryComponent data { get; set; }
        public Int32 privacy { get; set; }
        /// <summary>
        /// If true, this component is disabled.
        /// </summary>
        public bool? disabled { get; set; }
    }
}
