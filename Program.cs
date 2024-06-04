using System;
using System.Threading.Tasks;
using dotenv.net;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.Commands;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using Microsoft.Extensions.Configuration;


namespace wistJeDatBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new();
            // right click on config.json, click properties, and in Copy to Output Directory select Copy if newer
            configurationBuilder.AddJsonFile("config.json", true, true);
            configurationBuilder.AddCommandLine(args);

            IConfiguration configuration = configurationBuilder.Build();
            string discordToken = configuration.GetValue<string>("WISTJEDATBOT:discord_token") ?? throw new InvalidOperationException("Missing Discord token.");
            ulong debugGuildId = (ulong)configuration.GetValue<ulong?>("WISTJEDATBOT:debug_guild_id", null);
            string prefix = configuration.GetValue<string>("WISTJEDATBOT:prefix") ?? throw new InvalidOperationException("Missing prefix.");

            DiscordClientBuilder builder = DiscordClientBuilder.CreateDefault(discordToken, TextCommandProcessor.RequiredIntents | SlashCommandProcessor.RequiredIntents | DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents);
            DiscordClient discordClient = builder.Build();

            // Use the commands extension
            CommandsExtension commandsExtension = discordClient.UseCommands(new CommandsConfiguration()
            {
                //ServiceProvider = serviceProvider,
                DebugGuildId = debugGuildId,
                // The default value, however it's shown here for clarity
                RegisterDefaultCommandProcessors = true
            });

            // Add all commands by scanning the current assembly
            commandsExtension.AddCommands(typeof(Program).Assembly);
            TextCommandProcessor textCommandProcessor = new(new()
            {
                // If you want to change it, you first set if the bot should react to mentions
                // and then you can provide as many prefixes as you want.
                PrefixResolver = new DefaultPrefixResolver(true, "!").ResolvePrefixAsync
            });

            // Add text commands with a custom prefix (!ping)
            await commandsExtension.AddProcessorsAsync(textCommandProcessor);

            // We can specify a status for our bot. Let's set it to "online" and set the activity to "with fire".
            DiscordActivity status = new("for new facts", DiscordActivityType.Watching);

            // Now we connect and log in.
            await discordClient.ConnectAsync(status, DiscordUserStatus.Online);

            // And now we wait infinitely so that our bot actually stays connected.
            await Task.Delay(-1);
        }
    }
}