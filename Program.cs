using System;
using System.Threading.Tasks;
using dotenv.net;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace wist_je_dat_bot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Load the .env file.
            DotEnv.Load();

            // Grab discord token
            string? discordToken = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(discordToken))
            {
                Console.WriteLine("Please specify a token in the DISCORD_TOKEN environment variable.");
                Environment.Exit(1);

                // For the compiler's nullability, unreachable code.
                return;
            }

            // We instantiate our client.
            DiscordConfiguration config = new()
            {
                Token = discordToken,

                // Needs message content intent enabled in Discord Developer Portal.
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };

            DiscordClient client = new(config);

            // We can specify a status for our bot. Let's set it to "online" and set the activity to "with fire".
            DiscordActivity status = new("Grabbing ur daily facts", ActivityType.Playing);

            // Register Random as a singleton. This will be used by the random command.
            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(Random.Shared); // We're using the shared instance of Random for simplicity.

            // Register CommandsNext
            CommandsNextConfiguration commandsConfig = new()
            {
                // Add the service provider which will allow CommandsNext to inject the Random instance.
                Services = serviceCollection.BuildServiceProvider(),
                StringPrefixes = new[] { "!" }
            };
            CommandsNextExtension commandsNext = client.UseCommandsNext(commandsConfig);

            // Register commands
            // CommandsNext will search the assembly for any classes that inherit from BaseCommandModule and register them as commands.
            commandsNext.RegisterCommands(typeof(Program).Assembly);

            // Now we connect and log in.
            await client.ConnectAsync(status, UserStatus.Online);

            // And now we wait infinitely so that our bot actually stays connected.
            await Task.Delay(-1);
        }
    }
}