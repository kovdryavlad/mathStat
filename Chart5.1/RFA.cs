using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chart5._1
{
    public static class RFA
    {
        public static void Solve(Matrix R, double epsilon)
        {
            int wMin = getW(R);

            //eigenvalues {(1,0.6717,0.5639,0.2915,0.7731), (0.6717,1,0.4304,0.0171,0.3127),(0.5639,0.4304,1,0.1760,0.2589),(0.2915, 0.0171,0.1760,1,-0.0184), (0.7731, 0.3127, 0.2589, -0.0184, 1}
            //максимальной корреляции
            double[] h1Max = GetH1MaxCor(R.data);

            //Метод Триад
            double[] h2Triad = GetH2Triad(R);

            //метод усреднения
            double[] h3Averaging = GetH3Averaging(R);

            //центроидный метод
            double[] h4Centroid = GetH4Centroid(R);

            //Алероидный метод
            double[] h5Aleroid = GetH5Aleroid(R);

            //По Мгк
            double[] h6MGK = GetH6ByMGK(R);

            List<double[]> hCommon = new List<double[]>(new[] { h1Max, h2Triad, h3Averaging, h4Centroid, h5Aleroid, h6MGK });
            double[] f = FindFVector(R, wMin, hCommon);

            var fMin = (f.Select((value, index) => new { Value = value, Index = index }).OrderBy(el => el.Value).ToArray())[0];
            double[] hBest = hCommon[fMin.Index];

            int w = 0;
            Matrix RhPrev = R.SetMainDiagonal(hBest);
            Matrix Aprev = GetA(RhPrev, out w, wMin);
            double fPrev = fMin.Value;
            
            int n = R.Rows;

            //while (true)
            //while (true==true)
            while (true!=false)
            {
                Matrix A = GetA(RhPrev, out w, wMin);
                
                //w = Math.Max(w, wMin);

                //определяем общности
                double[] h = new double[n];

                for (int i = 0; i < n; i++)
                    h[i] = A.GetRow(i).GetCloneOfData().Take(w).Sum(el => el * el);
                
                //Matrix A = GetA(Rh, out w, wMin);

                Matrix Rzal = RhPrev - A * A.Transpose();

                RhPrev = A.SetMainDiagonal(h);
                
                double fCurr = 0;
                for (int v = 0; v < n; v++)
                    fCurr += Rzal.GetRow(v).GetCloneOfData().Where((val, index) => index != v).Sum(val => val * val);


                if (fCurr > fPrev)
                    break;

                int sumOfDifference = 0;
                Matrix ADifference = A - Aprev;
                double sum = 0;

                for (int i = 0; i < A.Rows; i++)
                    for (int j = 0; j < A.Columns; j++)
                        sum += Math.Pow(ADifference[i, j], 2);

                if (sum < epsilon)
                    break;
                
                if (sumOfDifference == A.Rows * A.Columns)
                    break;

                for (int i = 0; i < h.Length; i++)
                    if (h[i] * h[i] > 1)
                        break;

                ///***///
                //RhPrev = Rh;
                Aprev = A;
                fPrev = fCurr;
            }

        }

        private static double[] FindFVector(Matrix R, int wMin, List<double[]> hCommon)
        {
            //после нахождения предварительных оценок общностей
            var length = hCommon.Count;
            double[] f = new double[length];


            for (int i = 0; i < length; i++)
            {
                Matrix Rh = R.SetMainDiagonal(hCommon[i]);

                int w = 0;
                Matrix A = GetA(Rh, out w, wMin);

                Matrix Rzal = Rh - A * A.Transpose();

                w = Math.Max(w, wMin);

                double fValue = 0;

                for (int v = 0; v < R.Rows; v++)
                    //fValue += Rzal.GetRow(v).GetCloneOfData().Take(w).Where((val, index) => index != v).Sum(val => val * val);
                    fValue += Rzal.GetRow(v).GetCloneOfData().Where((val, index) => index != v).Sum(val => val * val);


                f[i] = fValue;
            }

            return f;
        }

        static int getW(Matrix R)
        {
            var l = LaverierFadeevaMethod.Solve(R, LaverierFadeevaSolvingOptions.FullSolving);
            LaverierFadeevaExtendedResult[] ext = LaverierFadeevaExtendedResult.ConvertToExtendedLaverierFadeevaResult(l);
            ext = ext.OrderByDescending(v => v.EigenValue.Abs()).ToArray();

            var eigenAverage = ext.Sum(v => v.EigenValue) / ext.Length;
            int w = ext.Count(v => v.EigenValue > eigenAverage);

            return w;
        }

        private static Matrix GetA(Matrix R, out int w, int wMin)
        {
            var l = LaverierFadeevaMethod.Solve(R, LaverierFadeevaSolvingOptions.FullSolving);
            LaverierFadeevaExtendedResult[] ext = LaverierFadeevaExtendedResult.ConvertToExtendedLaverierFadeevaResult(l);
            ext = ext.OrderByDescending(v => v.EigenValue.Abs()).ToArray();

            var eigenAverage = ext.Sum(v => v.EigenValue) / ext.Length;

            w = ext.Count(v => v.EigenValue> eigenAverage);

            w = Math.Max(w, wMin);

            var neededVectors = ext.Take(w).Select(el => el.eigenVector).ToList();

            Matrix A = Matrix.Create.JoinVectors(neededVectors);
            return A;
        }

        private static double[] GetH1MaxCor(double[][] R)
        {
            int n = R[0].Length;
            double[] result = new double[n];

            for (int i = 0; i < n; i++)
            {
                List<double> lst = new List<double>();
                for (int j = 0; j < n; j++) { 
                    lst.Add(R[i][j]);
                }

                result[i] = lst.OrderByDescending(v=>v).ToArray()[1];
            }

            return result;
        }

        private static double[] GetH2Triad(Matrix R)
        {
            int length = R.Columns;
            double[] result = new double[length];

            Func<int, int, int> getRealIndex = (index, k) => index < k ? index : index + 1;

            for (int k = 0; k < length; k++)
            {
                List<double> row = R.GetRow(k).GetCloneOfData().ToList();
                
                row.RemoveAt(k);

                var sequence = row.Select((value, index) => new { Value = value, Index = index }).OrderByDescending(el => el.Value).ToList();

                int i = getRealIndex(sequence[0].Index, k);
                int j = getRealIndex(sequence[1].Index, k);

                result[k] = Math.Abs((R[k, i] * R[k, j]) / R[i, j]);
            }

            return result;
        }

        private static double[] GetH3Averaging(Matrix R)
        {
            int length = R.Columns;
            double[] result = new double[length];

            for (int k = 0; k < length; k++)
            {
                List<double> row = R.GetRow(k).GetCloneOfData().ToList();

                row.RemoveAt(k);
                row = row.Select(el => el.Abs()).ToList(); 
                result[k] = row.Sum() / (length-1);
            }

            return result;
        }

        private static double[] GetH4Centroid(Matrix R)
        {
            var Rclone = (Matrix)R.Clone();

            int length = R.Columns;
            double[] result = new double[length];

            double sum = 0;

            for (int i = 0; i < length; i++)
            {
                Rclone[i, i] = R.GetRow(i).GetCloneOfData().Where((value, index) => index != i).Max();
                sum += Rclone.GetRow(i).GetCloneOfData().Sum(el => el.Abs());
            }
            

            for (int k = 0; k < length; k++)
            {
                List<double> row = Rclone.GetRow(k).GetCloneOfData().Select(el=>el.Abs()).ToList();

                result[k] = Math.Pow(row.Sum(),2) / sum;
            }

            return result;
        }


        private static double[] GetH5Aleroid(Matrix R)
        {
            int length = R.Columns;
            double[] result = new double[length];

            double sum = 0;

            for (int i = 0; i < length; i++)
                sum += R.GetRow(i).GetCloneOfData().Where((value, index) => index != i).Sum(el => el.Abs());


            for (int k = 0; k < length; k++)
            {
                List<double> row = R.GetRow(k).GetCloneOfData().Where((value, index) => index != k).Select(el => el.Abs()).ToList();

                result[k] = Math.Pow(row.Sum(), 2) / sum * ((double)length/(length-1));
            }

            return result;
        }


        private static double[] GetH6ByMGK(Matrix R)
        {
            var l = LaverierFadeevaMethod.Solve(R, LaverierFadeevaSolvingOptions.FullSolving);
            LaverierFadeevaExtendedResult[] ext = LaverierFadeevaExtendedResult.ConvertToExtendedLaverierFadeevaResult(l);
            ext = ext.OrderByDescending(v => v.EigenValue.Abs()).ToArray();

            int w = ext.Count(v => v.EigenValue.Abs() > 1);

            var neededVectors = ext.Take(w).Select(el => el.eigenVector).ToList();

            double[] result = new double[ext.Length];

            for (int i = 0; i < ext.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < w; j++)
                    sum += neededVectors[j][i];
                
                result[i] = sum;
            }


            return result;
        }
    }
}
