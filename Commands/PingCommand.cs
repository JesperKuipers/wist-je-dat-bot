using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Commands.Trees.Metadata;
using System.ComponentModel;

namespace wistJeDatBot.Commands
{
    public sealed class PingCommand
    {
        [Command("ping"), DescriptionAttribute("Pings the bot and returns the gateway latency."), TextAlias("pong")]
        public static ValueTask ExecuteAsync(CommandContext context) => context.RespondAsync($"Pong! Latency is {context.Client.Ping}ms.");
    }
}
