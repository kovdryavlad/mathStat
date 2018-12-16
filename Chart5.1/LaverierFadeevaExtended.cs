using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMatrix;

namespace Chart5._1
{
    class LaverierFadeevaExtendedResult
    {
        public double EigenValue;
        public Vector eigenVector;
        public string name;
        public bool includeInMGK;

        public static LaverierFadeevaExtendedResult[] ConvertToExtendedLaverierFadeevaResult(LaverrierFadeevaMethodResult laverierFadeevaResult)
        {
            int size = laverierFadeevaResult.EigenValues.Length;
            LaverierFadeevaExtendedResult[] result = new LaverierFadeevaExtendedResult[size];

            for (int i = 0; i < size; i++)
            {
                result[i] = new LaverierFadeevaExtendedResult()
                {
                    includeInMGK = false,
                    EigenValue = laverierFadeevaResult.EigenValues[i],
                    eigenVector = laverierFadeevaResult.EigenVectors[i].Normilize(),
                    name = "F" + (size-i-1)
                };
            }

            return result.OrderByDescending(v=>v.EigenValue).ToArray();
        }
    }
}
