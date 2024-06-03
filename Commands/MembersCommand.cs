using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;

namespace wistJeDatBot.Commands
{
    public sealed class MembersCommand : BaseCommandModule
    {
        // Register the method as a command, specifying the name and description.
        // Unfortunately, CommandsNext does not support static methods.
        [Command("members"), Description("Show amount of members in the server."), Aliases("users")]
        [SuppressMessage("Style", "IDE0022", Justification = "Paragraph.")]
        public async Task UsersAsync(CommandContext context)
        {
            // The CommandContext provides access to the DiscordClient, the message, the channel, etc.
            // If the CommandContext is not provided as a parameter, CommandsNext will ignore the method.
            // Additionally, without the CommandContext, it would be impossible to respond to the user.
            await context.RespondAsync($"This server has {context.Guild.MemberCount} members.");
        }
    }
}
