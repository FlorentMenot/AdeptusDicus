using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdeptusDicus.Entities;
using Discord;
using Shared.Base;
using Microsoft.Extensions.DependencyInjection;
using Shared.IReferentiel;
using Shared.Entities;

namespace AdeptusDicus.Modules
{
    public class CharactersModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IReferentiel<Caracteristic> _caracteristicService;
        private readonly IOriginReferentiel _originService;
        private readonly IFactionReferentiel _factionsService;

        public CharactersModule(IServiceProvider serviceProvider)
        {
            _caracteristicService = serviceProvider.GetRequiredService<IReferentiel<Caracteristic>>();
            _originService = serviceProvider.GetRequiredService<IOriginReferentiel>();
            _factionsService = serviceProvider.GetRequiredService<IFactionReferentiel>();
        }

        private readonly Dictionary<Role, string> _rolesTranslations = new()
        {
            { Role.Interlocuter, "Interlocuteur" },
            { Role.Mystic, "Mystique" },
            { Role.Savant, "Savant" },
            { Role.Penumbra, "Penumbra" },
            { Role.Warrior, "Guerrier" },
            { Role.Zealot, "Zélote" },
        };

        private readonly Dictionary<Role, string> _rolesBenefits = new()
        {
            { Role.Interlocuter, $@":skull: **Talents** : 4 au choix parmi Air of Authority,Briber, Gothic Gibberish, Dealmaker, Distracting, Gallows Humour, Lickspittle ou Overseer.{Environment.NewLine}:skull: **Compétences** : 3 avances à répartir en Conscience, Discipline, Intuition, Linguistique, Présence et Rapport{Environment.NewLine}:skull: **Spécialisations** : 2 avances en spécialisations d'Intuition, Présence ou Rapport{Environment.NewLine}:skull: **Équipement** : Couteau, Laspistol ou Stub Revolver, un kit de Survie, Vox Bead, et un au choix parmi Laud Hailer, Pict Recorder ou Vox-caster" },
            { Role.Mystic, $@":skull: **Talents** : Vous gagnez le talent Psyker si vous ne l'avez pas. Si vous l'avez déjà, vous gagnez 1 pouvoir psychique mineur et 1 pouvoir psychique d'une Discipline que vous connaissez. Choisissez 2 parmi : Condamner la sorcière, Destiné, Savoir interdit, Forteresse mental ou Psyker sanctionné {Environment.NewLine}:skull: **Compétences** : 3 avances à répartir en Connaissances, Conscience, Discipline, Intuition, Maîtrise Psychique et Navigation{Environment.NewLine}:skull: **Spécialisations** : 2 avances en spécialisations en Discipline (Peur), Linguistique (Interdit), Connaissances (Interdit), Conscience (Psyniscience), ou n'importe quelle spécialisation de Maîtrise Psychique. Si vous avez le talent Savoir Interdit, vous pouvez dépenser des avances dans n'importe quelle compétence (interdit) {Environment.NewLine}:skull: **Équipement** : Couteau ou bâton, Laspistol ou Stub Revolver, un kit de Survie, Vox Bead, et un Psy Focus ou un Auspex"},
            { Role.Savant, $@":skull: **Talents** : 2 au choix parmi Artiste, Assistant Attentif, Chirurgien, Data Delver, Mémoire eidétique et Lawbringer{Environment.NewLine}:skull: **Compétences** : 3 avances à répartir en Connaissances, Logique, Medicae, Navigation, Pilotage et Tech{Environment.NewLine}" },
            { Role.Penumbra, $@":skull: **Talents** : 2 au choix parmi Burglar, Familiar Terrain, Read Lips, Secret Identity, Skulker et Unremarkable{Environment.NewLine}:skull: **Compétences** : 3 avances à répartir en Athlétisme, Conscience, Dextérité, Furtivité, Réflexes et Tir{Environment.NewLine}:skull: **Spécialisations** : 2 avances en spécialisations de Furtivité, Réflexes et Tir{Environment.NewLine}:skull: **Équipement** : 2 Couteaux, Laspistol ou Autopistol, Lasgun ou fusil de sniper, un silencieux, une grenade Smoke, un kit de Survie et un Vox Bead, puis 2 parmi un Auspex, Comm Leech, Kit de déguisement, Drop Harness, un grappin et une corde, un set de Magnoculars, une Multikey, un Pict Recorder, un kit de Photo-visors ou un Signal Jammer" },
            { Role.Warrior, $@":skull: **Talents** : 2 au choix parmi Deadeye, Disarm, Drilled, Duellist, Tactical Movement et Two-Handed Cleave{Environment.NewLine}:skull: **Compétences** : 3 avances à répartir en Athlétisme, Medicae, Mêlée, Réflexes, Tir et Vigueur{Environment.NewLine}:skull: **Équipement** : Couteau, une épée ou une épée tronçonneuse, Laspistol ou Stub Revolver, Lasgun ou Shotgun (Combat), une grenade frag, une Scrap-Plate ou une Flak Jacket, un Backpack, un set de Survie et un Vox Bead" },
            { Role.Zealot, $@":skull: **Talents** : 2 au choix parmi Faithful(Imperial Cult), Flagellant, Frenzy, Hatred, Icon Bearer et Martyrdom{Environment.NewLine}:skull: **Compétences** : 3 avances à répartir en Connaissances, Discipline, Linguistique, Mêlée, Présence et Vigueur{Environment.NewLine}:skull: **Spécialisations** : 2 avances en spécialisations de Connaissances, Discipline ou Mêlée{Environment.NewLine}:skull: **Équipement** : Couteau, Great Weapon ou Épée tronçonneuse, Laspistol ou Hand Flamer, un set de Cuir Lourd ou un set de robes, une icône sainte, un Vox Bead, et un Laud Hailer ou un kit d'écriture" },
        };

        [SlashCommand("def-carac", "Obtient la correspondance entre En -> Fr ")]
        private async Task TranslateCaracteristics()
        {
            var sb = new StringBuilder();
            foreach (var skill in Enum.GetValues<Caracteristic>().OrderBy(c => c.ToString()))
            {
                sb.AppendLine($"*{skill}* → **{_caracteristicService.GetTranslation(skill)}**");
            }            
            await RespondAsync(sb.ToString());
            
        }

        [SlashCommand("rand-char", "Crée un personnage aléatoire")]
        public async Task CreateCharacterAsync(CharacterCreationStep step = CharacterCreationStep.Full)
        {
            List<Embed> embeds = new List<Embed>();
            int originDice = DiceHelper.D100();
            var characterOrigin = GetOrigin(originDice);
            int factionDice = DiceHelper.D100();
            var faction = GetFaction(factionDice, characterOrigin);
            int roleDice = RandomNumberGenerator.GetInt32(1, 7);
            var role = (Role)roleDice;
            if (step.HasFlag(CharacterCreationStep.Caracteristics) || step == CharacterCreationStep.Full)
            {
                embeds.Add(CaracteristicsBuilder().Build());
            }
            if (step.HasFlag(CharacterCreationStep.Origin) || step == CharacterCreationStep.Full)
            {
                embeds.Add(OriginBuilder(characterOrigin).Build());
            }

            if (step.HasFlag(CharacterCreationStep.Faction) || step == CharacterCreationStep.Full)
            {
                embeds.Add(FactionBuilder(faction).Build());
            }

            if (step.HasFlag(CharacterCreationStep.Role) || step == CharacterCreationStep.Full)
            {
                embeds.Add(RoleBuilder(role).Build());
            }
            await RespondAsync($"{Context.Interaction.User.Mention}", embeds.ToArray());
        }

        private EmbedBuilder CaracteristicsBuilder()
        {
            var sbCaract = new StringBuilder();
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int dice1 = DiceHelper.D10();
                int dice2 = DiceHelper.D10();
                sbCaract.Append($"2D10 + 20 : {dice1} + {dice2} + 20 → __{dice1+dice2+20}__{Environment.NewLine}");
                sum += dice1 + dice2 + 20;
            }
            return new EmbedBuilder()
                .WithTitle($"Caractéristiques")
                .WithDescription(sbCaract.ToString())
                .WithFields(new EmbedFieldBuilder().WithName("Total").WithValue($"**{sum}**"))
                .WithColor(Color.DarkPurple);
        }

        private EmbedBuilder OriginBuilder(Origin characterOrigin)
        {                                    
            return new EmbedBuilder()
                .WithTitle($"**Origine : {_originService.GetTranslation(characterOrigin)}**")
                .WithDescription($"{_originService.GetBonus(characterOrigin)}")
                .WithFields(new EmbedFieldBuilder().WithName("Équipement").WithValue($"*{_originService.GetEquipement(characterOrigin)}*"))
                .WithColor(Color.DarkRed);
        }

        private EmbedBuilder FactionBuilder(Faction characterFaction)
        {
            return new EmbedBuilder()
                .WithTitle($"**Faction : {_factionsService.GetTranslation(characterFaction)}**")
                .WithDescription($"{_factionsService.GetBenefits(characterFaction)}")
                .WithFields(new EmbedFieldBuilder().WithName("Exemples").WithValue($"*{_factionsService.GetDuty(characterFaction)}*"))
                .WithColor(Color.Green);
        }

        private EmbedBuilder RoleBuilder(Role characterRole)
        {
            return new EmbedBuilder()
                .WithTitle($"**Rôle : {_rolesTranslations[characterRole]}**")
                .WithDescription($"{_rolesBenefits[characterRole]}")
                .WithColor(Color.DarkerGrey);
        }

        private Origin GetOrigin(int originDice)
        {
            switch (originDice)
            {
                case >= 1 and <= 10:
                    return Origin.AgriWorld;
                case >= 11 and <= 20:
                    return Origin.FeudalWorld;
                case >= 21 and <= 30:
                    return Origin.FeralWorld;
                case >= 31 and <= 40:
                    return Origin.ForgeWorld;
                case >= 41 and <= 70:
                    return Origin.HiveWorld;
                case >= 71 and <= 80:
                    return Origin.ShrineWorld;
                case >= 81 and <= 90:
                    return Origin.ScholaProgenium;
                case >= 91 and <= 100:
                    return Origin.Voidborn;
            }

            throw new UnreachableException("La valeur entrée est inférieure à 0 ou supérieure à 100");
        }

        private Faction GetFaction(int factionDice, Origin origin)
        {
            switch (origin)
            {
                case Origin.AgriWorld:
                    {
                        switch (factionDice)
                        {
                            case 1:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 2 and <= 11:
                                return Faction.AdeptusMechanicus;
                            case >= 12 and <= 37:
                                return Faction.Administratum;
                            case >= 38 and <= 51:
                                return Faction.AstraMilitarum;
                            case >= 52 and <= 65:
                                return Faction.AdeptusMinistorum;
                            case >= 66 and <= 67:
                                return Faction.Inquisition;
                            case >= 68 and <= 79:
                                return Faction.ImperialFleet;
                            case >= 80 and <= 98:
                                return Faction.Infractionnist;
                            case >= 99 and <= 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }
                case Origin.FeudalWorld:
                    {
                        switch (factionDice)
                        {
                            case 1:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 2 and <= 3:
                                return Faction.AdeptusMechanicus;
                            case >= 4 and <= 29:
                                return Faction.Administratum;
                            case >= 30 and <= 63:
                                return Faction.AstraMilitarum;
                            case >= 64 and <= 79:
                                return Faction.AdeptusMinistorum;
                            case >= 80 and <= 81:
                                return Faction.Inquisition;
                            case >= 82 and <= 85:
                                return Faction.ImperialFleet;
                            case >= 86 and <= 98:
                                return Faction.Infractionnist;
                            case >= 99 and <= 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }

                case Origin.FeralWorld:
                    {
                        switch (factionDice)
                        {
                            case 1:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 2 and <= 3:
                                return Faction.AdeptusMechanicus;
                            case >= 4 and <= 17:
                                return Faction.Administratum;
                            case >= 18 and <= 69:
                                return Faction.AstraMilitarum;
                            case >= 70 and <= 82:
                                return Faction.AdeptusMinistorum;
                            case >= 83 and <= 84:
                                return Faction.Inquisition;
                            case >= 85 and <= 94:
                                return Faction.ImperialFleet;
                            case >= 95 and <= 99:
                                return Faction.Infractionnist;
                            case 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }
                case Origin.ForgeWorld:
                    {
                        switch (factionDice)
                        {
                            case 1:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 2 and <= 51:
                                return Faction.AdeptusMechanicus;
                            case >= 52 and <= 71:
                                return Faction.Administratum;
                            case 72:
                                return Faction.AstraMilitarum;
                            case >= 73 and <= 74:
                                return Faction.AdeptusMinistorum;
                            case 75:
                                return Faction.Inquisition;
                            case >= 76 and <= 85:
                                return Faction.ImperialFleet;
                            case >= 86 and <= 99:
                                return Faction.Infractionnist;
                            case 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }
                case Origin.HiveWorld:
                    {
                        switch (factionDice)
                        {
                            case 1:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 2 and <= 13:
                                return Faction.AdeptusMechanicus;
                            case >= 14 and <= 37:
                                return Faction.Administratum;
                            case >= 38 and <= 57:
                                return Faction.AstraMilitarum;
                            case >= 58 and <= 69:
                                return Faction.AdeptusMinistorum;
                            case >= 70 and <= 71:
                                return Faction.Inquisition;
                            case >= 72 and <= 81:
                                return Faction.ImperialFleet;
                            case >= 82 and <= 98:
                                return Faction.Infractionnist;
                            case >= 99 and <= 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }
                case Origin.ShrineWorld:
                    {
                        switch (factionDice)
                        {
                            case 1:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 2 and <= 3:
                                return Faction.AdeptusMechanicus;
                            case >= 4 and <= 17:
                                return Faction.Administratum;
                            case >= 18 and <= 31:
                                return Faction.AstraMilitarum;
                            case >= 32 and <= 71:
                                return Faction.AdeptusMinistorum;
                            case >= 72 and <= 73:
                                return Faction.Inquisition;
                            case >= 74 and <= 85:
                                return Faction.ImperialFleet;
                            case >= 86 and <= 99:
                                return Faction.Infractionnist;
                            case 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }
                case Origin.ScholaProgenium:
                    {
                        switch (factionDice)
                        {
                            case 1:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 2 and <= 3:
                                return Faction.AdeptusMechanicus;
                            case >= 4 and <= 23:
                                return Faction.Administratum;
                            case >= 24 and <= 45:
                                return Faction.AstraMilitarum;
                            case >= 46 and <= 67:
                                return Faction.AdeptusMinistorum;
                            case >= 68 and <= 87:
                                return Faction.Inquisition;
                            case >= 88 and <= 94:
                                return Faction.ImperialFleet;
                            case >= 95 and <= 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }
                case Origin.Voidborn:
                    {
                        switch (factionDice)
                        {
                            case >= 1 and <= 10:
                                return Faction.AdeptusAstraTelepathica;
                            case >= 11 and <= 20:
                                return Faction.AdeptusMechanicus;
                            case >= 21 and <= 34:
                                return Faction.Administratum;
                            case >= 35 and <= 48:
                                return Faction.AstraMilitarum;
                            case >= 49 and <= 52:
                                return Faction.AdeptusMinistorum;
                            case >= 53 and <= 54:
                                return Faction.Inquisition;
                            case >= 55 and <= 84:
                                return Faction.ImperialFleet;
                            case >= 85 and <= 89:
                                return Faction.Infractionnist;
                            case >= 90 and <= 100:
                                return Faction.RogueTraderDynasty;
                        }

                        break;
                    }
            }
            throw new UnreachableException("La valeur entrée est inférieure à 0 ou supérieure à 100");
        }
    }
}
