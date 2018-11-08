using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart1._1
{
    public partial class STAT
    {
        public void Sdandartize()
        {
            for (int i = 0; i < d.Length; i++)
            {
                d[i] = (d[i] - Expectation) / Sigma;
            }
        }
        
        public void Zsuv(double zsuv)
        {
            for (int i = 0; i < d.Length; i++)
                d[i] = d[i] + zsuv; 
        }

        public void Logarifmirovat(double osnova)
        {
            if (Min < 1)
                this.Zsuv(Math.Abs(Min) +1.5);

            for (int i = 0; i < d.Length; i++)
                d[i] = Math.Log(d[i], osnova);
        }

        public void PidnesennyaDoStepenya(double stepen)
        {
            d.Select(x => x = Math.Pow(x, stepen));
        }
    }
}
