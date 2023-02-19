using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Destiny.ResponseTypes.Destiny.Components;

namespace MerpBot.Destiny.ResponseTypes.Destiny.Responses
{
    public class DestinyCharacterResponse
    {
        public SingleComponentResponseOfDestinyInventoryComponent inventory { get; set; } // why do these have to have such long names lol
        /*public SingleComponentResponseOfDestinyCharacterComponent character { get; set; }
        public SingleComponentResponseOfDestinyCharacterProgressionComponent progressions { get; set; }
        public SingleComponentResponseOfDestinyCharacterRenderComponent renderData { get; set; }
        public SingleComponentResponseOfDestinyCharacterActivitiesComponent activities { get; set; }*/
        public SingleComponentResponseOfDestinyInventoryComponent equipment { get; set; }
        /*public SingleComponentResponseOfDestinyKiosksComponent kiosks { get; set; }
        public SingleComponentResponseOfDestinyPlugSetsComponent plugSets { get; set; }
        public SingleComponentResponseOfDestinyPresentationNodesComponent presentationNodes { get; set; }
        public SingleComponentResponseOfDestinyCharacterRecordsComponent records { get; set; }
        public SingleComponentResponseOfDestinyCollectiblesComponent collectibles { get; set; }
        public DestinyItemComponentSetOfint64 itemComponents { get; set; }
        public DestinyBaseItemComponentSetOfuint32 uninstancedItemComponents { get; set; }
        public SingleComponentResponseOfDestinyCurrenciesComponent currencyLookups { get; set; }*/
    }
}
