using AdeptusDicus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Shared.Entities;
using Shared.IReferentiel;

namespace Referentiel
{
    public class OriginReferentiel : IOriginReferentiel
    {
        private readonly Dictionary<Origin, string> _originsTranslations = new()
        {
            { Origin.AgriWorld, "Monde agricole" },
            { Origin.FeudalWorld, "Monde féodal" },
            { Origin.FeralWorld, "Monde sauvage" },
            { Origin.ForgeWorld, "Monde forge" },
            { Origin.HiveWorld, "Monde ruche" },
            { Origin.ShrineWorld, "Monde tombeau" },
            { Origin.ScholaProgenium, "ScholaProgenium" },
            { Origin.Voidborn, "Né dans le vide" }
        };

        private readonly Dictionary<Origin, string> _originsBonus = new();

        private readonly Dictionary<Origin, string> _originsEquipement = new()
        {
            { Origin.AgriWorld, "Outils de forage de piète qualité" },
            { Origin.FeudalWorld, "Outils d'écriture de piètre qualité" },
            { Origin.FeralWorld, "Équipements de survie de piètre qualité" },
            { Origin.ForgeWorld, "Fiole d'Onguents Sacrés" },
            { Origin.HiveWorld, "Bouchons filtrants laids" },
            { Origin.ShrineWorld, "Sainte Icône" },
            { Origin.ScholaProgenium, "Chrono" },
            { Origin.Voidborn, "Bottes magnétiques de piètre qualité" }
        };

        public OriginReferentiel(IServiceProvider serviceProvider)
        {
            var caracteristicServices = serviceProvider.GetRequiredService<IReferentiel<Caracteristic>>();
            _originsBonus.Add(Origin.AgriWorld, $"+5 {caracteristicServices.GetTranslation(Caracteristic.Strengh)} et +5 {caracteristicServices.GetTranslation(Caracteristic.Agility)}, {caracteristicServices.GetTranslation(Caracteristic.Toughness)} ou {caracteristicServices.GetTranslation(Caracteristic.Willpower)}");
            _originsBonus.Add(Origin.FeudalWorld, $"+5 {caracteristicServices.GetTranslation(Caracteristic.WeaponSkill)} et +5 {caracteristicServices.GetTranslation(Caracteristic.Fellowship)}, {caracteristicServices.GetTranslation(Caracteristic.Strengh)} ou {caracteristicServices.GetTranslation(Caracteristic.Willpower)}");
            _originsBonus.Add(Origin.FeralWorld, $"+5 {caracteristicServices.GetTranslation(Caracteristic.Toughness)} et +5 {caracteristicServices.GetTranslation(Caracteristic.WeaponSkill)}, {caracteristicServices.GetTranslation(Caracteristic.Strengh)} ou {caracteristicServices.GetTranslation(Caracteristic.Perception)}");
            _originsBonus.Add(Origin.ForgeWorld, $"+5 {caracteristicServices.GetTranslation(Caracteristic.Intelligence)} et +5 {caracteristicServices.GetTranslation(Caracteristic.Agility)}, {caracteristicServices.GetTranslation(Caracteristic.WeaponSkill)} ou {caracteristicServices.GetTranslation(Caracteristic.Toughness)}");
            _originsBonus.Add(Origin.HiveWorld, $"+5 {caracteristicServices.GetTranslation(Caracteristic.Agility)} et +5 {caracteristicServices.GetTranslation(Caracteristic.Fellowship)}, {caracteristicServices.GetTranslation(Caracteristic.WeaponSkill)} ou {caracteristicServices.GetTranslation(Caracteristic.Perception)}");
            _originsBonus.Add(Origin.ShrineWorld, $"+5 {caracteristicServices.GetTranslation(Caracteristic.Willpower)} et +5 {caracteristicServices.GetTranslation(Caracteristic.Fellowship)}, {caracteristicServices.GetTranslation(Caracteristic.Intelligence)} ou {caracteristicServices.GetTranslation(Caracteristic.Perception)}");
            _originsBonus.Add(Origin.ScholaProgenium, $"+5 {caracteristicServices.GetTranslation(Caracteristic.Fellowship)} et +5 {caracteristicServices.GetTranslation(Caracteristic.WeaponSkill)}, {caracteristicServices.GetTranslation(Caracteristic.BallisticSkill)} ou {caracteristicServices.GetTranslation(Caracteristic.Toughness)}");
            _originsBonus.Add(Origin.Voidborn, $"+5 {caracteristicServices.GetTranslation(Caracteristic.Perception)} et +5 {caracteristicServices.GetTranslation(Caracteristic.Agility)}, {caracteristicServices.GetTranslation(Caracteristic.Intelligence)} ou {caracteristicServices.GetTranslation(Caracteristic.Willpower)}");            
        }

        public IList<Origin> Items => Enum.GetValues<Origin>();

        public string GetBonus(Origin origin)
        {
            return _originsBonus[origin];
        }

        public string GetEquipement(Origin origin)
        {
            return _originsEquipement[origin];
        }

        public string GetTranslation(Origin item)
        {
            return _originsTranslations[item];
        }
    }
}
