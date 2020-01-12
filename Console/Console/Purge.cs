using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DiscordConsole
{
    public class Purge : ModuleBase<SocketCommandContext>
    {
        [Command("purge")]
        public async Task DelMessageAsync(int delnum)
        {
            var items = await Context.Channel.GetMessagesAsync(delnum + 1).FlattenAsync();
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(items.ToList());
        }

        [Command("purge")]
        public async Task DelMessageAsync(IUser user, int delnum)
        {
            var request = await Context.Channel.GetMessagesAsync(1).FlattenAsync();
            await Context.Channel.DeleteMessageAsync(request.First());


            var items = await Context.Channel.GetMessagesAsync(delnum).FlattenAsync();
            var messages = items.Where(msg => msg.Author.Equals(user));
            var counter = 2;
            var prevMessagesLength = messages.Count();
            var sinceLastMessage = 0;
            while (messages.Count() != delnum)
            {
                items = await Context.Channel.GetMessagesAsync(delnum + counter++).FlattenAsync();
                messages = items.Where(msg => msg.Author.Equals(user));
                if (messages.Count() == prevMessagesLength)
                {
                    sinceLastMessage++;
                }
                else
                {
                    prevMessagesLength = messages.Count();
                    sinceLastMessage = 0;
                }

                if (sinceLastMessage > 20)
                    break;
            }


            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages.ToList());

        }

    }
}
