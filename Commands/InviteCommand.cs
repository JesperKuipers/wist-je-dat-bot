using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Entities;
using static System.Net.WebRequestMethods;

namespace wistJeDatBot.Commands
{
    /// <summary>
    /// Copied from https://github.com/OoLunar/Tomoe/blob/master/src/Commands/Common/InviteCommand.cs
    /// </summary>
    public static class InviteCommand
    {
        /// <summary>
        /// Sends the Discord Authorization link to add the bot to your server.
        /// </summary>
        [Command("invite"), Description("Sends the link to get cool 😎 facts.")]
        public static ValueTask InviteAsync(CommandContext context)
        {
            //DiscordPermissions requiredPermissions = GetSubcommandsPermissions(context.Extension.Commands.Values);
            DiscordPermissions requiredPermissions = (DiscordPermissions)1099511725127;
            StringBuilder stringBuilder = new();
            stringBuilder.Append("<https://discord.com/api/oauth2/authorize?client_id=");
            stringBuilder.Append(context.Client.CurrentUser.Id);
            stringBuilder.Append("&scope=bot%20applications.commands");
            if (requiredPermissions != 0)
            {
                stringBuilder.Append("&permissions=");
                stringBuilder.Append((long)requiredPermissions);
            }

            stringBuilder.Append('>');
            return context.RespondAsync(stringBuilder.ToString());
        }

        private static DiscordPermissions GetSubcommandsPermissions(IEnumerable<Command> subCommands)
        {
            DiscordPermissions permissions = 0;
            foreach (Command subCommand in subCommands)
            {
                if (subCommand.Subcommands.Count != 0)
                {
                    permissions |= GetSubcommandsPermissions(subCommand.Subcommands);
                }

                if (subCommand.Attributes.OfType<RequirePermissionsAttribute>().FirstOrDefault() is RequirePermissionsAttribute permissionsAttribute)
                {
                    permissions |= permissionsAttribute.BotPermissions;
                }
            }

            return permissions;
        }
    }
}