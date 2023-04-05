using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerpBot.Interactions.Interfaces;
public interface ISnowflake : IEntity<ulong>
{
    public ulong DiscordEpoch { get; }
    public ulong ToUnixTimestampMilliseconds();
    public ulong ToUnixTimestampSeconds();
}

public class Snowflake : ISnowflake
{
    public Snowflake(ulong id)
    {
        Id = id;
    }

    public ulong DiscordEpoch { get; } = 1420070400000;
    public ulong Id { get; }

    public ulong ToUnixTimestampMilliseconds()
    {
        ulong timestamp = (Id >> 22) + DiscordEpoch;

        return timestamp;
    }

    public ulong ToUnixTimestampSeconds()
    {
        ulong timestamp = (Id >> 22) + DiscordEpoch;

        return (ulong)Math.Floor((double)timestamp/1000);
    }
}

public sealed class SnowflakeConverter<T> : TypeConverter<T> where T : ISnowflake
{
    public override ApplicationCommandOptionType GetDiscordType() => ApplicationCommandOptionType.String;

    public override Task<TypeConverterResult> ReadAsync(IInteractionContext context, IApplicationCommandInteractionDataOption option, IServiceProvider services)
    {
        if (ulong.TryParse((string)option.Value, out var result))
            return Task.FromResult(TypeConverterResult.FromSuccess(new Snowflake(result)));
        else
            return Task.FromResult(TypeConverterResult.FromError(InteractionCommandError.ConvertFailed, $"Value {option.Value} cannot be converted to {nameof(T)}"));
    }
}
