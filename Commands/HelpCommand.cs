using System.ComponentModel;
using DSharpPlus.Entities;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Commands.Trees.Metadata;

namespace wistJeDatBot.Commands
{
    public sealed class HelpCommand
    {
        [Command("help"), Description("Shows all commands"), TextAlias("commands")]
        public ValueTask ExecuteAsync(CommandContext context)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Orange,
                Description = "**All commands:**"
            };

            foreach (var command in context.Extension.Commands)
            {
                embedBuilder.AddField(command.Value.FullName, command.Value.Description ?? "No description available", false);
            }

            var responseBuilder = new DiscordMessageBuilder()
                .AddEmbed(embedBuilder);

            return context.RespondAsync(embedBuilder);
        }
    }
}
