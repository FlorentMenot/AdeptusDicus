using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Base
{
    public static class DiceHelper
    {
        public static int D100()
        {
            return RandomNumberGenerator.GetInt32(1, 101);
        }

        public static int D10()
        {
            return RandomNumberGenerator.GetInt32(1, 11);
        }
    }
}
