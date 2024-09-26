using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;

namespace AdeptusDicus.Modules
{
    public class CommonModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Ping le bot et affiche la latence")]
        public async Task GreetUserAsync()
            => await RespondAsync(text: $":ping_pong: Cela m'a pris {Context.Client.Latency}ms à vous répondre!", ephemeral: false);
    }
}
