using System.ComponentModel;
using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Commands.Trees.Metadata;
using DSharpPlus.Entities;

namespace wistJeDatBot.Commands
{
    /// <summary>
    /// copied from https://github.com/OoLunar/Tomoe/blob/master/src/Commands/Common/FlipCommand.cs
    /// </summary>
    public static class FlipCommand
    {
        /// <summary>
        /// Heads or tails?
        /// </summary>
        [Command("coinflip"), TextAlias("flip", "random"), Description("Coinflip."), RequirePermissions(DiscordPermissions.EmbedLinks, DiscordPermissions.None)]
        public static async ValueTask ExecuteAsync(CommandContext context)
        {
            await context.DeferResponseAsync();
            await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 8)));
            await context.RespondAsync(Random.Shared.Next(2) == 0 ? "Heads" : "Tails");
        }
    }
}