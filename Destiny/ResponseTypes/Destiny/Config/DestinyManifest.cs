using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Destiny.ResponseTypes.Destiny.Config
{
    public class DestinyManifest
    {
        public string version { get; set; }
        public string mobileAssetContentPath { get; set; }
        public GearAssetDataBaseDefinition[] mobileGearAssetDataBases { get; set; }
        public Dictionary<string, string> mobileWorldContentPaths { get; set; }
        public Dictionary<string, string> jsonWorldContentPaths { get; set; }
        public Dictionary<string, object> jsonWorldComponentContentPaths { get; set; }
        public string mobileClanBannerDatabasePath { get; set; }
        public Dictionary<string, string> mobileGearCDN { get; set; }
        public ImagePyramidEntry[] iconImagePyramidInfo { get; set; }
    }
}
