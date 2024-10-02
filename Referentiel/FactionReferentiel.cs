using AdeptusDicus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shared.Base;
using Shared.Entities;
using Shared.IReferentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Referentiel
{
    public class FactionReferentiel : IFactionReferentiel
    {

        private readonly Dictionary<Faction, string> _factionsTranslation = new()
        {
            { Faction.AdeptusAstraTelepathica, "Adeptus Astra Telepathica" },
            { Faction.AdeptusMechanicus, "Adeptus Mechanicus" },
            { Faction.Administratum, "Administratum" },
            { Faction.AstraMilitarum, "Astra Militarum" },
            { Faction.AdeptusMinistorum, "Adeptus Ministorum" },
            { Faction.ImperialFleet, "Flotte impériale " },
            { Faction.Infractionnist, "Infractionniste" },
            { Faction.Inquisition, "Inquisition" },
            { Faction.RogueTraderDynasty, "Dynastie de libre marchand" },
        };

        private readonly Dictionary<Faction, string> _factionsBenefits;

        private readonly Dictionary<Faction, string> _factionsDuty = new()
        {
            { Faction.AdeptusAstraTelepathica, "Membre d'équipage d'un vaisseau noir, Psyker sanctionné, Soeur en attente, ..." },
            { Faction.AdeptusMechanicus, "Apprenti ingénieur, Apprenti Genetor, Apprenti Logis, ..." },
            { Faction.Administratum, "Clerc, Officio Medicae, Scribe" },
            { Faction.AstraMilitarum, "Spécialiste de mêlée, Éclaireur, Homme de troupe" },
            { Faction.AdeptusMinistorum, "Missionnaire, Prêcheur, Soeur novice, ..." },
            { Faction.ImperialFleet, "Pilote, Homme d'armes, Officier de la Navis, ..." },
            { Faction.Infractionnist, "Nettoyeur, Membre de gangs, Coureur de ruches, ..." },
            { Faction.Inquisition, "Acolyte, Exorciste, Sage, ..." },
            { Faction.RogueTraderDynasty, "Aventurier, Cataloguiste, Diplomate, ..." },
        };

        public FactionReferentiel(IServiceProvider serviceProvider)
        {
            var caracteristicService = serviceProvider.GetRequiredService<IReferentiel<Caracteristic>>();
            var skillService = serviceProvider.GetRequiredService<IReferentiel<Skill>>();
            _factionsBenefits = new()
            {
                { Faction.AdeptusAstraTelepathica, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Willpower)} et +5 {caracteristicService.GetTranslation(Caracteristic.Toughness)}, {caracteristicService.GetTranslation(Caracteristic.Intelligence)} ou {caracteristicService.GetTranslation(Caracteristic.Perception)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Awareness)}, {skillService.GetTranslation(Skill.Discipline)}, {skillService.GetTranslation(Skill.Intuition)}, {skillService.GetTranslation(Skill.Linguistics)}, {skillService.GetTranslation(Skill.PsychicMastery)} et {skillService.GetTranslation(Skill.Navigation)}{Environment.NewLine}:skull: +1 en Influence avec Adeptus Astra Telepathica{Environment.NewLine}:skull: 1 au choix : Psyker et Psyker sanctionné; Blank; Condamner la sorcière et Forteresse mentale{Environment.NewLine}:skull: Couteau avec la modification Mono-arête, un ensemble de robes, un instrument de divination et 500 solars"},
                { Faction.AdeptusMechanicus, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Intelligence)} et +5 {caracteristicService.GetTranslation(Caracteristic.Agility)}, {caracteristicService.GetTranslation(Caracteristic.Toughness)} ou {caracteristicService.GetTranslation(Caracteristic.Perception)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Lore)}, {skillService.GetTranslation(Skill.Dexterity)}, {skillService.GetTranslation(Skill.Logic)}, {skillService.GetTranslation(Skill.Medicae)}, {skillService.GetTranslation(Skill.Piloting)} et {skillService.GetTranslation(Skill.Tech)}{Environment.NewLine}:skull: +1 en Influence avec Adeptus Mechanicus{Environment.NewLine}:skull: 1 au choix : Auspex, Implante vocal, Système respiratoire augmentique, Organe sensoriel augmentique (yeux, nez, oreilles..){Environment.NewLine}:skull: 1 au choix : {Environment.NewLine}:skull: Dataslate, un ensemble de robes, une fiole d'onguent sacrés et 100 solars"},
                { Faction.Administratum, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Intelligence)} et +5 {caracteristicService.GetTranslation(Caracteristic.Fellowship)}, {caracteristicService.GetTranslation(Caracteristic.Perception)} ou {caracteristicService.GetTranslation(Caracteristic.Willpower)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Lore)}, {skillService.GetTranslation(Skill.Dexterity)}, {skillService.GetTranslation(Skill.Linguistics)}, {skillService.GetTranslation(Skill.Medicae)} et {skillService.GetTranslation(Skill.Navigation)}{Environment.NewLine}:skull: +1 en Influence avec Administratum et 1 Contact{Environment.NewLine}:skull: Talent : Data Delver{Environment.NewLine}:skull: 1 au choix : Auto-quill, un kit de chirurgie, un Pict recorder, Vox Caster{Environment.NewLine}:skull: Dataslate, un ensemble de robes, un kit d'écriture et 800 solars"},
                { Faction.AstraMilitarum, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Toughness)} et +5 {caracteristicService.GetTranslation(Caracteristic.WeaponSkill)}, {caracteristicService.GetTranslation(Caracteristic.BallisticSkill)} ou {caracteristicService.GetTranslation(Caracteristic.Strengh)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Athletics)}, {skillService.GetTranslation(Skill.Discipline)}, {skillService.GetTranslation(Skill.Stealth)}, {skillService.GetTranslation(Skill.Melee)}, {skillService.GetTranslation(Skill.Ranged)} et {skillService.GetTranslation(Skill.Fortitude)}{Environment.NewLine}:skull: +1 en Influence avec Astra Militarum{Environment.NewLine}:skull: Talent : Éduqué{Environment.NewLine}:skull: 2 parmi : Lasgun, Laspistol, Carabine laser, Outils de retranchement, Cape Caméléon ou Épée tronçonneuse{Environment.NewLine}:skull: Couteau, armure flak de l'Astra Militarum, grenade frag et 300 solars"},
                { Faction.AdeptusMinistorum, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Willpower)} et +5 {caracteristicService.GetTranslation(Caracteristic.Fellowship)}, {caracteristicService.GetTranslation(Caracteristic.Intelligence)} ou {caracteristicService.GetTranslation(Caracteristic.Perception)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Lore)}, {skillService.GetTranslation(Skill.Discipline)}, {skillService.GetTranslation(Skill.Intuition)}, {skillService.GetTranslation(Skill.Medicae)}, {skillService.GetTranslation(Skill.Presence)} et {skillService.GetTranslation(Skill.Rapport)}{Environment.NewLine}:skull: +1 en Influence avec Ecclesiarchy.{Environment.NewLine}:skull: Talent : Croyant véritable (Culte impérial){Environment.NewLine}:skull: 2 au choix : Plastron carapace, une épée tronçonneuse, un kit de chirurgien, un bâton ornemental ou un ensemble de robes de cérémonie{Environment.NewLine}:skull: Un ensemble de robes, une icône sainte, ensemble d'élingues et 600 solars"},
                { Faction.ImperialFleet, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Agility)} et +5 {caracteristicService.GetTranslation(Caracteristic.Toughness)}, {caracteristicService.GetTranslation(Caracteristic.Strengh)} ou {caracteristicService.GetTranslation(Caracteristic.Perception)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Awareness)}, {skillService.GetTranslation(Skill.Logic)}, {skillService.GetTranslation(Skill.Navigation)}, {skillService.GetTranslation(Skill.Piloting)}, {skillService.GetTranslation(Skill.Reflexes)} et {skillService.GetTranslation(Skill.Tech)}{Environment.NewLine}:skull: +1 en Influence avec Navis Imperialis{Environment.NewLine}:skull: Talent : Void Legs{Environment.NewLine}:skull: Laspistol ou Fusil à pompe, Void Suit ou Veste Flak et Bottes magnétiques, Lascutter ou Photo-Visors et 500 solars"},
                { Faction.Infractionnist, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Agility)} et +5 {caracteristicService.GetTranslation(Caracteristic.Fellowship)}, {caracteristicService.GetTranslation(Caracteristic.Toughness)} ou {caracteristicService.GetTranslation(Caracteristic.Perception)} {Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Athletics)}, {skillService.GetTranslation(Skill.Dexterity)}, {skillService.GetTranslation(Skill.Stealth)}, {skillService.GetTranslation(Skill.Rapport)}, {skillService.GetTranslation(Skill.Reflexes)}, {skillService.GetTranslation(Skill.Fortitude)}{Environment.NewLine}:skull: +1 en Influence avec Infractionists{Environment.NewLine}:skull: Talent : Bien-préparé{Environment.NewLine}:skull: Stub Automatic ou Stub Revolver; Cuir Léger ou Cuir Lourd{Environment.NewLine}:skull: Une arme banale et un outil commun, les deux ont le modificateur Laid et Piètre qualité{Environment.NewLine}:skull: Couteau, un sac à dos et 5d10 {DiceHelper.D10() + DiceHelper.D10() + DiceHelper.D10() + DiceHelper.D10() + DiceHelper.D10()} solars"},
                { Faction.Inquisition, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Perception)} et +5 {caracteristicService.GetTranslation(Caracteristic.Toughness)}, {caracteristicService.GetTranslation(Caracteristic.Intelligence)} ou {caracteristicService.GetTranslation(Caracteristic.Willpower)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Lore)}, {skillService.GetTranslation(Skill.Awareness)}, {skillService.GetTranslation(Skill.Discipline)}, {skillService.GetTranslation(Skill.Intuition)}, {skillService.GetTranslation(Skill.Logic)}, {skillService.GetTranslation(Skill.Presence)}{Environment.NewLine}:skull: +1 en Influence avec Inquisition.{Environment.NewLine}:skull: Talent : Toujours Vigilant{Environment.NewLine}:skull: 1 au choix : Auspex, Comm Leech ou un Pict recorder{Environment.NewLine}:skull: Laspistol, Gilet de protection blindé, un globe lumineux, une paire de menottes et 400 solars"},
                { Faction.RogueTraderDynasty, $@":skull: +5 {caracteristicService.GetTranslation(Caracteristic.Fellowship)} et +5 {caracteristicService.GetTranslation(Caracteristic.Agility)}, {caracteristicService.GetTranslation(Caracteristic.Intelligence)} ou {caracteristicService.GetTranslation(Caracteristic.Perception)}{Environment.NewLine}:skull: 5 avances à répartir en {skillService.GetTranslation(Skill.Intuition)}, {skillService.GetTranslation(Skill.Linguistics)}, {skillService.GetTranslation(Skill.Navigation)}, {skillService.GetTranslation(Skill.Presence)}, {skillService.GetTranslation(Skill.Piloting)} et {skillService.GetTranslation(Skill.Rapport)}{Environment.NewLine}:skull: +1 en Influence avec Rogue Trader Dynasties {Environment.NewLine}:skull: Talent : Dealmaker{Environment.NewLine}:skull: Gilet de protection blindé, un Multicompass et 1200 solars"},
            };
        }

        public IList<Faction> Items => Enum.GetValues<Faction>();

        public string GetBenefits(Faction faction)
        {
            return _factionsBenefits[faction];
        }

        public string GetDuty(Faction faction)
        {
            return _factionsDuty[faction];
        }

        public string GetTranslation(Faction item)
        {
            return _factionsTranslation[item];
        }
    }
}
