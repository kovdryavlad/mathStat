﻿using System;
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

        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        public static double Pow(this double value, double p)
        {
            return Math.Pow(value, p);
        }
    }
}
