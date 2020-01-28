using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LastHubConsole.Data
{
    public static class GuildData
    {
        public static readonly ConcurrentBag<SocketCategoryChannel> LockedCategories = new ConcurrentBag<SocketCategoryChannel>();
        


        
    }
}
