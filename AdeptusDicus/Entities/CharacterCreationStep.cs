using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdeptusDicus.Entities
{
    [Flags]
    public enum CharacterCreationStep
    {
        Full = 0,
        Caracteristics = 1,
        Origin = 2,
        Faction = 4,
        Role = 8,
    }
}
