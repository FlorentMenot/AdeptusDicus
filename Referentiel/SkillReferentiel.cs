using AdeptusDicus.Entities;
using Shared.Entities;
using Shared.IReferentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Referentiel
{
    public class SkillReferentiel : IReferentiel<Skill>
    {
        private readonly Dictionary<Skill, string> _skillsTranslations = new()
        {           
            { Skill.Athletics, "Athlétisme" },
            { Skill.Awareness, "Conscience" },
            { Skill.Dexterity, "Dextérité" },
            { Skill.Discipline, "Discipline" },
            { Skill.Fortitude, "Vigueur" },
            { Skill.Intuition, "Intuition" },
            { Skill.Linguistics, "Linguistique" },
            { Skill.Logic, "Logique" },
            { Skill.Lore, "Connaissances" },
            { Skill.Medicae, "Medicae" },
            { Skill.Melee, "Mêlée" },
            { Skill.Navigation, "Navigation" },
            { Skill.Presence, "Présence" },
            { Skill.Piloting, "Pilotage" },
            { Skill.PsychicMastery, "Maîtrise psychique" },
            { Skill.Ranged, "Tir" },
            { Skill.Rapport, "Rapport" },
            { Skill.Reflexes, "Réflexes" },
            { Skill.Stealth, "Furtivité" },
            { Skill.Tech, "Tech" },
        };

        public IList<Skill> Items => Enum.GetValues<Skill>();

        public string GetTranslation(Skill item)
        {
            return _skillsTranslations[item];
        }
    }
}
