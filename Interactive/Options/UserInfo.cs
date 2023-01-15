using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;

namespace MerpBot.Interactive.Options
{
    public enum UserInfoOptions : int
    {
        [ChoiceDisplay("Simple")]
        simple,
        [ChoiceDisplay("Extra")]
        extra,
        [ChoiceDisplay("Extensive")]
        extensive
    }
}
