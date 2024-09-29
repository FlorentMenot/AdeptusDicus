using AdeptusDicus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IReferentiel
{
    public interface IOriginReferentiel : IReferentiel<Origin>
    {
        string GetBonus(Origin origin);

        string GetEquipement(Origin origin);
    }
}
