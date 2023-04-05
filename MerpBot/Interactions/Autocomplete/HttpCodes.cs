using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MerpBot.Interactions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MerpBot.Interactions.Autocomplete;
public class HttpCodes : AutocompleteHandler
{

    public async override Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
    {
        HttpStatusCodeExtention[] codes = Enum.GetValues<HttpStatusCodeExtention>();

        AutocompleteResult[] result = new AutocompleteResult[codes.Length];

        string userInput = autocompleteInteraction.Data.Current.Value.ToString() ?? string.Empty;

        for (int i = 0; i < codes.Length; i++)
        {
            result[i] = new AutocompleteResult($"{codes[i]} ({(int)codes[i]})", (int)codes[i]);
        }

        return AutocompletionResult.FromSuccess(result.Where(x => x.Name.Contains(userInput, StringComparison.InvariantCultureIgnoreCase)).Take(25));
    }
}
