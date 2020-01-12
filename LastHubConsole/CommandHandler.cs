using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Discord;
using System.Reflection;

namespace LastHubConsole
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        //private readonly IConfiguration config;
        private readonly IServiceProvider services;

        public CommandHandler(IServiceProvider services)
        {
            //this.config = services.GetRequiredService<IConfiguration>();
            this.commands = services.GetRequiredService<CommandService>();
            this.client = services.GetRequiredService<DiscordSocketClient>();
            this.services = services;

            client.MessageReceived += HandleCommandsAsync;

            commands.CommandExecuted += CommandExecutedAsync;


        }

        public async Task InstallCommandsAsync()
        {
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleCommandsAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null)
                return;

            int argPos = 0;
            if (!message.HasStringPrefix("$", ref argPos) || message.Author.IsBot)
                return;

            var context = new SocketCommandContext(client, message);

            await commands.ExecuteAsync(context, argPos, null);
        }



        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified)
                return;

            if (result.IsSuccess)
                return;

            await context.Channel.SendMessageAsync("Something went wrong");
        }
    }
}