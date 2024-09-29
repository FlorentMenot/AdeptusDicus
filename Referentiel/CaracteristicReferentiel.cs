using Shared.Entities;
using Shared.IReferentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Referentiel
{
    public class CaracteristicReferentiel : IReferentiel<Caracteristic>
    {
        public readonly Dictionary<Caracteristic, string> _caracteristicsTranslation = new()
        {
            { Caracteristic.WeaponSkill, "Capacité de Combat" },
            { Caracteristic.BallisticSkill, "Capacité de Tir" },
            { Caracteristic.Strengh, "Force" },
            { Caracteristic.Toughness, "Endurance" },
            { Caracteristic.Agility, "Agilité" },
            { Caracteristic.Intelligence, "Intelligence" },
            { Caracteristic.Perception, "Perception" },
            { Caracteristic.Willpower, "Volonté" },
            { Caracteristic.Fellowship, "Communauté" }
        };

        public IList<Caracteristic> Items => Enum.GetValues<Caracteristic>();

        public string GetTranslation(Caracteristic item)
        {
            return _caracteristicsTranslation[item];
        }
    }
}
