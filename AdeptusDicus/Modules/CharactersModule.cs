using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdeptusDicus.Entities;

namespace AdeptusDicus.Modules
{
    public class CharactersModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Dictionary<Origin, string> _origins = new()
        {
            { Origin.AgriWorld, "Monde agricole : +5 Force et +5 Agilité, Endurance ou Volonté" },
            { Origin.FeudalWorld, "Monde féodal : +5 CC et +5 Communauté, Force ou Volonté" },
            { Origin.FeralWorld, "Monde sauvage : +5 Endurance et +5 CC, Force ou Perception" },
            { Origin.ForgeWorld, "Monde forge : +5 Intelligence et +5 Agilité, CT ou Endurance" },
            { Origin.HiveWorld, "Monde ruche : +5 Agilité et +5 Communauté, CT ou Perception" },
            { Origin.ShrineWorld, "Monde tombeau : +5 Volonté et +5 Communauté, Intelligence ou Perception" },
            { Origin.ScholaProgenium, "ScholaProgenium (Orphelinat) : +5 Communauté et +5 CC, CT ou endurance" },
            { Origin.Voidborn, "Né dans le vide : +5 Perception et +5 Agilité, Intelligence ou Volonté" }
        };

        private readonly Dictionary<Faction, string> _factions = new()
        {
            {
                Faction.AdeptusAstraTelepathica,
                "Adeptus Astra Telepathica - Recherche, entraînement et utilisation des psykers"
            },
            {
                Faction.AdeptusMechanicus,
                "Adeptus Mechanicus - Maintien des machines du culte de Mars (Vaisseaux, Chantiers spatiaux, etc)"
            },
            { Faction.Administratum, "Administratum - Scribes, Curateurs, Bureaucrates, Notaires, ...." },
            { Faction.AstraMilitarum, "Astra Militarum - Le marteau de l'empereur" },
            { Faction.AdeptusMinistorum, "Adeptus Ministorum - Chargés du culte impériale, défense planétaire, ..." },
            {
                Faction.ImperialFleet,
                "La flotte impériale - Pilote, officier de quart, simple marin, réparateur de vaisseau, etc"
            },
            {
                Faction.Infractionnist,
                "Contrevenant - Accusé par défaut, membre d'un gang ou franc-tireur, vous vivez en marge de la loi"
            },
            {
                Faction.Inquisition,
                "L'Inquisition - Chargé de la traque et de la purge des hérétiques, des mutants, des xénos, des hérétiques xénos ou des mutants hérétiques"
            },
            {
                Faction.RogueTraderDynasty,
                "Dynastie de marchand libre - Contrebandier ? Maître de caravanes ? Pirate ? Recycleur d'épave ? Un peu tout à la fois. Je me définis plutôt comme un opportuniste"
            },
        };

        private readonly Dictionary<Role, string> _roles = new()
        {
            { Role.Interlocuter, "Interlocuteur - Vous préférez les mots aux armes." },
            { Role.Mystic, "Mystique - Vous êtes un psyker" },
            { Role.Savant, "Savant - Vous connaissez de nombreuses choses sans forcément les mettre en pratique" },
            { Role.Penumbra, "Penumbra - Vous ne me voyez que parce que je l'ai décidé" },
            { Role.Warrior, "Guerrier - Épée, bolt, fusil laser, etc...." },
            { Role.Zealot, "Zélote - Vous ne faites jamais les choses à moitié et êtes animés par la foi (Omni-messie, Dieu de Mars, etc)" }
        };

        [SlashCommand("rand-char", "Crée un personnage aléatoire")]
        public async Task CreateCharacterAsync()
        {
            //WS - Weapon Skill = Capacité de Combat (CC)
            //BS - Ballistic Skill = Capacité de Tir (CT)
            //STR - Strengh = Force (For)
            //TGH - Toughness = Endurance (End)
            //AG - Agility = Agilité (Ag)
            //INT - Intelligence = Intelligence (Int)
            //PER - Perception = Perception (PER)
            //WIL - Willpower = Volonté (Vol)
            //FEL - Fellowship = Communauté (COM)
            var sb = new StringBuilder();
            sb.Append($"Caractéristiques à répartir {Environment.NewLine} ");
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int roll = DiceHelper.D10() + DiceHelper.D10() + 20;
                sum += roll;
                sb.Append(roll);
                if (i < 8)
                {
                    sb.Append(" - ");
                }
            }

            sb.Append($" - **Total ({sum})**{Environment.NewLine}");
            int originDice = DiceHelper.D100();
            var characterOrigin = GetOrigin(originDice);
            sb.AppendLine($"Origine ({originDice}) - *{_origins[characterOrigin]}*");
            int factionDice = DiceHelper.D100();
            var faction = GetFaction(factionDice, characterOrigin);
            sb.AppendLine($"Faction ({factionDice}) - *{_factions[faction]}*");
            int roleDice = RandomNumberGenerator.GetInt32(1, 7);
            var role = (Role)roleDice;
            sb.AppendLine($"Rôle ({roleDice}) - *{_roles[role]}*");
            await RespondAsync(text: sb.ToString());
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
