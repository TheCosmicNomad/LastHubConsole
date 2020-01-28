using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LastHubConsole.Data
{
    public static class UserData
    {
        public static ConcurrentDictionary<SocketUser, int> VoiceChatTokens = new ConcurrentDictionary<SocketUser, int>();


        public static async Task OnUserJoin(SocketUser user)
        {
            VoiceChatTokens.TryAdd(user, 0);
            await Task.CompletedTask;
        }

    }
}
