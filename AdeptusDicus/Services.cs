using Microsoft.Extensions.DependencyInjection;
using Referentiel;
using Shared.Entities;
using Shared.IReferentiel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdeptusDicus
{
    internal class Services
    {
        internal static void AddReferentielServices(IServiceCollection collection)
        {
            collection.AddSingleton<IReferentiel<Caracteristic>, CaracteristicReferentiel>();
            collection.AddSingleton<IOriginReferentiel, OriginReferentiel>();
        }

    }
}
