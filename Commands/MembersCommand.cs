using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Commands.Trees.Metadata;
using System.ComponentModel;

namespace wistJeDatBot.Commands
{
    public sealed class MembersCommand
    {
        [Command("members"), Description("Show amount of members in the server."), TextAlias("users")]
        public static ValueTask ExecuteAsync(CommandContext context) => context.RespondAsync($"This server has {context.Guild.MemberCount} members.");
    }
}
