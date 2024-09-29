using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IReferentiel
{
    public interface IReferentiel<T>
    {
        IList<T> Items { get; }

        string GetTranslation(T item);
    }
}
