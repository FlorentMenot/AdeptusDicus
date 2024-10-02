using AdeptusDicus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IReferentiel
{
    public interface IFactionReferentiel : IReferentiel<Faction>
    {
        string GetBenefits(Faction faction);

        string GetDuty(Faction faction);
    }
}
