using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DiscordConsole
{
    public class Program
    {

        //private readonly IConfiguration config;
        private DiscordSocketClient client;

        

        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                // get the client and assign to client 
                // you get the services via GetRequiredService<T>
                var client = services.GetRequiredService<DiscordSocketClient>();
                this.client = client;

                // setup logging and the ready event
                client.Log += Log;

                services.GetRequiredService<CommandService>().Log += Log;

                // this is where we get the Token value from the configuration file, and start the bot
                await client.LoginAsync(TokenType.Bot, "NjU3NDYyMzEzNDE4NDg5ODY5.Xfxjiw.iYYjgKNdVxvSRELctetMaqDnPGo");
                await client.StartAsync();

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InstallCommandsAsync();

                await Task.Delay(-1);
            }
        }

        private Task Client_UserVoiceStateUpdated(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3) => throw new NotImplementedException();

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }


        private ServiceProvider ConfigureServices()
        {
            // this returns a ServiceProvider that is used later to call for those services
            // we can add types we have access to here, hence adding the new using statement:
            // using csharpi.Services;
            // the config we build is also added, which comes in handy for setting the command prefix!
            return new ServiceCollection()
                //.AddSingleton(config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }


    }


    
}
