using Discord.Interactions;

namespace AdeptusDicus.Modules
{
    public class DiceModule : InteractionModuleBase<InteractionContext>
    {
        public enum RollType
        {
            Classic,
            Advantage,
            Disadvantage
        }

        [SlashCommand("roll", "Basic Roll")]
        public async Task RollCommand(RollType rollType)
        {
            await RespondAsync($"Do a barrell roll {rollType}");
        }
    }
}
