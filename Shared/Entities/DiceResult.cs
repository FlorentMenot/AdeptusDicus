using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace AdeptusDicus.Entities
{
    public class DiceResult
    {
        public string TextResult { get; set; } = string.Empty;

        public Color ColorResult { get; set; }

        public Result Result { get; set; }

        public bool IsCritical { get; set; }
    }
}
