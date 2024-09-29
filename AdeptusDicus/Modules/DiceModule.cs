using AdeptusDicus.Entities;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Shared.Base;
using System.Security.Cryptography;
using System.Text;

namespace AdeptusDicus.Modules
{
    public class DiceModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("roll", "Jet de dés")]
        public async Task RollCommandAsync(RollType rollType = RollType.Classic, int difficulté = -1, int valeur = -1, string description = "")
        {
            var diceResult = GetResult(rollType, difficulté, valeur, description);
            string title = GetTitle(diceResult);
            var embedBuilder = new EmbedBuilder()
                .WithTitle(title)
                .WithDescription(diceResult.TextResult)
                .WithColor(diceResult.ColorResult);

            await RespondAsync($"{Context.Interaction.User.Mention}", embed: embedBuilder.Build());
        }

        private string GetTitle(DiceResult diceResult)
        {
            if (diceResult.IsCritical)
            {
                if (diceResult.Result == Result.Undetermined)
                {
                    return "***Critique***";
                }

                if (diceResult.Result == Result.Failure)
                {
                    return "***Échec critique !!***";
                }

                if (diceResult.Result == Result.Success)
                {
                    return "***Succès critique !!***";
                }
            }

            if (diceResult.Result == Result.Success)
            {
                return "**Succès**";
            }

            if (diceResult.Result == Result.Failure)
            {
                return "**Échec**";
            }

            return "**Résultat**";
        }

        [SlashCommand("r", "Jet de dé libre")]
        public async Task AnyRollAsync(string dice)
        {
            try
            {
                var diceFormat = dice.Substring(1, dice.Length - 1);
                int diceNumber = Int32.Parse(diceFormat);
                if (diceNumber < 1)
                {
                    await RespondAsync($"Désolé mais je n'ai pas compris la commande {dice}");
                }
                int result = RandomNumberGenerator.GetInt32(1, diceNumber + 1);
                await RespondAsync($"{Context.Interaction.User.Mention} a lancé un {dice} : {result}");
            }
            catch (Exception e)
            {
                await RespondAsync($"Désolé mais je n'ai pas compris la commande {dice} : {e.Message}");
            }
        }

        private int InvertDice(IntDecomposition decomposition)
        {
            return decomposition.Hundred * 100 + decomposition.Unit * 10 + decomposition.Ten;
        }

        private bool CheckCritical(IntDecomposition decomposition)
        {
            return decomposition.Ten == decomposition.Unit && decomposition.Hundred != 1;
        }

        private IntDecomposition DecomposeInt32(int diceResult)
        {
            var decomposition = new IntDecomposition();
            decomposition.Hundred = (int)diceResult / 100;
            diceResult = diceResult - decomposition.Hundred * 100;
            decomposition.Ten = diceResult / 10;
            diceResult = diceResult - decomposition.Ten * 10;
            decomposition.Unit = diceResult;
            return decomposition;
        }

        private DiceResult GetResult(RollType rollType, int difficulty, int threshold, string description)
        {
            var result = new DiceResult();
            result.ColorResult = Color.Blue;
            result.IsCritical = false;
            result.Result = Result.Undetermined;
            var diceResult = DiceHelper.D100();
            var sb = new StringBuilder();
            sb.AppendLine($"Jet : {diceResult}");


            if (difficulty != -1 || threshold != -1)
            {
                string difficultyText = difficulty != -1 ? $"Difficulté : {difficulty}" : string.Empty;
                string thresholdText = threshold != -1 ? $"Score : {threshold}" : string.Empty;
                if (difficulty != -1 && threshold != -1)
                {
                    sb.AppendLine($"{difficultyText} - {thresholdText}");
                }
                else if (difficulty == -1)
                {
                    sb.AppendLine($"{thresholdText}");
                }
                else if (threshold == -1)
                {
                    sb.AppendLine($"{difficultyText}");
                }
            }


            if (!string.IsNullOrWhiteSpace(description))
            {
                sb.AppendLine($"*{description}*");
            }
            var decomposition = DecomposeInt32(diceResult);
            if (rollType == RollType.Advantage && decomposition.Ten > decomposition.Unit)
            {
                diceResult = InvertDice(decomposition);
                sb.AppendLine($"Vous aviez un avantage ! Ce jet devient : {diceResult}");
            }

            if (rollType == RollType.Disadvantage && decomposition.Unit > decomposition.Ten)
            {
                diceResult = InvertDice(decomposition);
                sb.AppendLine($"Vous aviez un désavantage ! Ce jet devient : {diceResult}");
            }

            bool isCritical = CheckCritical(decomposition);
            result.IsCritical = isCritical;
            if (isCritical && diceResult < 90 && threshold == -1)
            {
                result.ColorResult = Color.Gold;
            }

            if (diceResult < 6)
            {
                result.ColorResult = Color.Green;
                result.Result = Result.Success;
                sb.AppendLine("Un jet de dès entre 01 et 05 est toujours un succès !");
            }
            else if (diceResult > 95)
            {
                result.Result = Result.Failure;
                result.ColorResult = isCritical ? Color.DarkRed : Color.Red;
                sb.AppendLine(isCritical
                    ? $"Oh oh ! {diceResult} est un fumble automatique !"
                    : "Oh ! Tout jet entre 96 et 100 est toujours un échec");
            }
            else
            {
                if (difficulty != -1 && threshold == -1)
                {
                    sb.AppendLine(
                        $"La difficulté étant de {difficulty}, considérez cela comme un {diceResult - difficulty} pour savoir si c'est un échec ou un succès");
                }
                else if (threshold != -1)
                {
                    if (difficulty != -1)
                    {
                        threshold += difficulty;
                    }
                    string resultString = string.Empty;
                    if (diceResult <= threshold)
                    {
                        result.Result = Result.Success;
                        var success = DecomposeInt32(threshold-diceResult);
                        int sl = success.Ten;
                        result.ColorResult = isCritical ? Color.DarkGreen : Color.Green;

                        if (sl == 0)
                        {
                            resultString = $"Succès Marginal, SL : {sl}";
                        }

                        if (sl is 1 or 2)
                        {
                            resultString = $"Succès, SL : {sl}";
                        }

                        if (sl is 3 or 4)
                        {
                            resultString = $"Succès impressionnant ! SL : {sl}";
                        }

                        if (sl >= 5)
                        {
                            resultString = $"Succès retentissant !! SL : {sl}";
                        }

                        sb.AppendLine(isCritical ? "Succès critique !" : resultString);
                    }
                    else
                    {
                        result.Result = Result.Failure;
                        var fail = DecomposeInt32(diceResult-threshold);
                        int sl = fail.Ten;
                        result.ColorResult = isCritical ? Color.DarkRed : Color.Red;
                        if (sl == 0)
                        {
                            resultString = $"Échec Marginal, SL : {sl}";
                        }

                        if (sl is 1 or 2)
                        {
                            resultString = $"Échec, SL : {sl}";
                        }

                        if (sl is 3 or 4)
                        {
                            resultString = $"Échec impressionnant ! SL : {sl}";
                        }

                        if (sl >= 5)
                        {
                            resultString = $"Échec retentissant !! SL : {sl}";
                        }

                        sb.AppendLine(isCritical ? "Échec critique !" : resultString);
                    }
                }
            }
            result.TextResult = sb.ToString();
            return result;
        }
    }
}
