namespace MerpBot.Destiny.ResponseTypes.Destiny
{
    public class DestinyComponentType
    {
        public enum None { None = 0 }
        public enum Profiles
        {
            Profiles = 100,
            VendorReceipts = 101,
            ProfileInventories = 102,
            ProfileCurrencies = 103,
            ProfileProgression = 104,
            PlatformSilver = 105,
        }
        public enum Characters
        {
            /// <summary>
            /// This will get you summary info about each of the characters in the profile.
            /// </summary>
            Characters = 200,
            /// <summary>
            /// This will get you information about any non-equipped items on the character or character(s) in question, if you're allowed to see it. 
            /// You have to either be authenticated as that user, or that user must allow anonymous viewing of their non-equipped items in Bungie.Net settings to actually get results.
            /// </summary>
            CharacterInventories = 201,
            /// <summary>
            /// This will get you information about the progression (faction, experience, etc... "levels") relevant to each character, if you are the currently authenticated user or the user has elected to allow anonymous viewing of its progression info.
            /// </summary>
            CharacterProgressions = 202,
            /// <summary>
            /// This will get you just enough information to be able to render the character in 3D if you have written a 3D rendering library for Destiny Characters, or "borrowed" ours. 
            /// It's okay, I won't tell anyone if you're using it. I'm no snitch. (actually, we don't care if you use it - go to town)
            /// </summary>
            CharacterRenderData = 203,
            /// <summary>
            /// This will return info about activities that a user can see and gating on it, if you are the currently authenticated user or the user has elected to allow anonymous viewing of its progression info. 
            /// Note that the data returned by this can be unfortunately problematic and relatively unreliable in some cases. 
            /// We'll eventually work on making it more consistently reliable.
            /// </summary>
            CharacterActivities = 204,
            /// <summary>
            /// This will return info about the equipped items on the character(s). 
            /// Everyone can see this.
            /// </summary>
            CharacterEquipment = 205,
        }
        public enum Items
        {
            ItemInstances = 300,
            ItemObjecives = 301,
            ItemPerks = 302,
            ItemRenderData = 303,
            ItemStats = 304,
            ItemSockets = 305,
            ItemTalentGrids = 306,
            ItemCommonData = 307,
            ItemPlugStates = 308,
            ItemPlugObjectives = 309,
            ItemReusablePlugs = 310,
        }
        public enum Vendors
        {
            Vendors = 400,
            VendorCategories = 401,
            VendorSales = 402,
        }
        public enum Other
        {
            Kiosks = 500,
            CurrencyLookups = 600,
            PresentationNodes = 700,
            Collectibles = 800,
            Records = 900,
            Transitory = 1000,
            Metrics = 1100,
            StringVariables = 1200,
            Craftables = 1300
        }
    }
}
