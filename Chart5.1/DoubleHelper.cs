using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    public static class DoubleHelper
    {
        public static double Round(this double value, int digitsAfterComma)
        {
            return Math.Round(value, digitsAfterComma);
        }
    }
}
