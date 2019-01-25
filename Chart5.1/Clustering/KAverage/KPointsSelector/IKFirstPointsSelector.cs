using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1.KAverage
{
    public interface IKFirstPointsSelector
    {
        double[][] GetFirstKPoints(double[][] data, int k);
    }
}
