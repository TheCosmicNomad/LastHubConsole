using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using LastHubConsole.Data;
using System.Globalization;

namespace LastHubConsole.Commands
{
    [Group("category")]
    [Alias("cat")]
    public class VoiceChat : ModuleBase<SocketCommandContext>
    {
        [Command("add")]
        [Alias("create")]
        public async Task CreateChannel(string _name, int count = 1)
        {
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;

            var user = Context.User as SocketGuildUser;

            string name = ti.ToTitleCase(_name);

            if (user.GuildPermissions.Administrator)
            {
                var category = await Context.Guild.CreateCategoryChannelAsync(name);
                for (int i = 1 ; i <= count ; i++)
                {
                    var channel = await Context.Guild.CreateVoiceChannelAsync($"{name.ToLower()} {i}", d => d.CategoryId = category.Id);
                }
            }
            
            else if (UserData.VoiceChatTokens.GetOrAdd(Context.User, 0) > 0)
            {
                await ReplyAsync("Sorry, you do not have enough tokens to create a channel");
            }
            
            else if (Context.Guild.CategoryChannels.Where(c => c.Name.Equals(name)).Count() > 0)
            {
                await ReplyAsync("This category already exists");
            }

            else
            {
                var category = await Context.Guild.CreateCategoryChannelAsync(name);
                for (int i = 1 ; i <= count ; i++)
                {
                    var channel = await Context.Guild.CreateVoiceChannelAsync($"{name.ToLower()} {i}", d => d.CategoryId = category.Id);
                }
                await ReplyAsync($"{name.ToUpper()} was created successfully. You now have **{UserData.VoiceChatTokens[user]--}** token(s) remaining");
            }
        }

        [Command("delete")]
        [Alias("del", "remove")]
        public async Task DeleteChannel(string name)
        {
            var category = Context.Guild.CategoryChannels.First(c => c.Name.ToLower().Equals(name.ToLower()));
            
            var user = Context.User as SocketGuildUser;

            if (category == null)
            {
                await ReplyAsync("This category does not exist");
            }
            else if (user.GuildPermissions.Administrator)
            {
                foreach(var channel in category.Channels)
                {
                    await channel.DeleteAsync();
                }
                await category.DeleteAsync();
                await ReplyAsync($"{name.ToUpper()} was deleted successfully");
            }
            else if (GuildData.LockedCategories.Contains(category))
            {
                await ReplyAsync("This category is locked");
            }
        }

        [Command("modify")]
        [Alias("mod")]
        public async Task ModifyChannel(string name, int count)
        {

        }



        [Command("lock")]
        [Alias("keep", "l")]
        public async Task LockChannel(string name)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.Administrator)
            {
                var channel = Context.Guild.CategoryChannels.First(c => c.Name.ToLower().Equals(name.ToLower()));
                if (channel == null)
                {
                    await ReplyAsync("This category does not exist");
                }
                else
                {
                    GuildData.LockedCategories.Add(channel);
                    await ReplyAsync($"{name.ToUpper()} successfully locked");
                }
            }
            else
            {
                await ReplyAsync("You do not have permission to lock channels");
            }
        }

        [Command("unlock")]
        [Alias("release", "ul")]
        public async Task UnlockChannel(string name)
        {
            var user = Context.User as SocketGuildUser;
            if (user.GuildPermissions.Administrator)
            {
                var channel = Context.Guild.CategoryChannels.First(c => c.Name.ToLower().Equals(name.ToLower()));
                if (channel == null)
                {
                    await ReplyAsync("This category does not exist");
                }

                if (GuildData.LockedCategories.TryTake(out channel))
                {
                    await ReplyAsync($"{name.ToUpper()} successfully unlocked");
                }
                else
                {
                    await ReplyAsync($"Something went wrong when trying to unlock {name.ToUpper()}");
                }
                
            }
            else
            {
                await ReplyAsync("You do not have permission to unlock channels");
            }
        }

    }
}

