using Chart1._1;
using SimpleMatrix;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chart5._1.TimeData
{
    public static class CaterpillarMethodHelper
    {
        public static (Matrix A, Matrix Y) Decomposition_ForPred(STAT _sample, int M, IEnumerable<int> indexes = null)
        {
            int N = _sample.d.Length;
            var xArray = new double[M][];
            var p = _sample.d;

            for (int i = 0; i < xArray.Length; i++)
            {
                xArray[i] = new double[N - M + 1];
                for (int j = 0; j < xArray[i].Length; j++)
                {
                    xArray[i][j] = p[i + j];
                }
            }

            Matrix X = new Matrix(xArray);
            Matrix DC = X * X.Transpose();

            var _eagleResults = DC.GetEigenResult(0.00001);

            List<(int index, double value, Vector vector)> valuevectorPairs = _eagleResults.EValues.Select((value, index) => (index, value, _eagleResults.EVectors[index]))
                //.OrderByDescending(vr => vr.value).Take(M)
                .ToList();

            Matrix A = Matrix.Create.JoinVectors(valuevectorPairs.Select(vv => vv.vector));

            return (A, A * X);
        }


        public static EValuesVectorResult Decomposition_Eigen(STAT _sample, int M)
        {
            int N = _sample.d.Length;
            var xArray = new double[M][];
            var p = _sample.d;

            for (int i = 0; i < xArray.Length; i++)
            {
                xArray[i] = new double[N - M + 1];
                for (int j = 0; j < xArray[i].Length; j++)
                {
                    xArray[i][j] = p[i + j];
                }
            }

            Matrix X = new Matrix(xArray);
            Matrix DC = X * X.Transpose();

            var result = DC.GetEigenResult(0.00001);
            return result;
        }

        public static (Matrix A, Matrix Y) Decomposition_ForReconstr(STAT _sample, int M, IEnumerable<int> indexes = null)
        {
            int N = _sample.d.Length;
            var xArray = new double[M][];
            var p = _sample.d;

            for (int i = 0; i < xArray.Length; i++)
            {
                xArray[i] = new double[N - M + 1];
                for (int j = 0; j < xArray[i].Length; j++)
                {
                    xArray[i][j] = p[i + j];
                }
            }

            Matrix X = new Matrix(xArray);
            Matrix DC = X * X.Transpose();

            var _eagleResults = DC.GetEigenResult(0.00001);

            List<(int index, double value, Vector vector)> valuevectorPairs = _eagleResults.EValues.Select((value, index) => (index, value, _eagleResults.EVectors[index]))
                .OrderByDescending(vr => vr.value).Take(M)
                .ToList();
            if (indexes != null)
            {
                List<(int index, double value, Vector vector)> temp = new List<(int index, double value, Vector vector)>();
                for (int i = 0; i < indexes.Count(); i++)
                {
                    temp.Add(valuevectorPairs[indexes.ElementAt(i) - 1]);
                }
                valuevectorPairs = temp;
            }
            Matrix A = null;
            if (indexes.Count() == 1)
            {
                A = Matrix.Create.New(new double[1][] {
                    valuevectorPairs[0].vector.GetCloneOfData()
                }).Transpose();
            }
            else
                A = Matrix.Create.JoinVectors(valuevectorPairs.Select(vv => vv.vector));

            return (A, X.Transpose() * A);
        }
        
        public static List<double> Reconstruction_ForPredict(STAT _sample, int M, IEnumerable<int> indexes)
        {
            var (a, y) = Decomposition_ForPred(_sample, M);
            double[,] newX = new double[y.Rows, y.Columns];
            if (indexes == null)
                indexes = Enumerable.Range(0, y.Rows);
            else indexes = indexes.Select(i => i - 1);
            for (int i = 0; i < newX.GetLength(0); i++)
            {
                for (int j = 0; j < newX.GetLength(1); j++)
                {
                    newX[i, j] = indexes.Select(v => a[v, i] * y[v, j]).Sum();
                }
            }

            List<double> P = new List<double>();
            for (int i = 0; i < a.Rows; i++)
                P.Add(Enumerable.Range(0, i + 1).Select(j => newX[j, i - j]).Sum() / (i + 1));
            for (int i = a.Rows; i < y.Columns; i++)
                P.Add(Enumerable.Range(0, a.Rows).Select(j => newX[j, i - j]).Sum() / a.Rows);
            for (int i = 1; i < y.Rows; i++)
                P.Add(Enumerable.Range(i, y.Rows - i).Select(j => newX[j, i - j + y.Columns - 1]).Sum() / (y.Rows - i));
            return P.ToList();

        }

        public static double Prediction(STAT _sample, int M)
        {
            var (a, y) = Decomposition_ForPred(_sample, M);
            var p = Reconstruction_ForPredict(_sample, M, null);
            int N = p.Count;
            double[,] xmatrix = new double[y.Rows, y.Columns];
            for (int i = 0; i < xmatrix.GetLength(0); i++)
                for (int j = 0; j < xmatrix.GetLength(1); j++)
                    xmatrix[i, j] = p[i + j];

            double newEl = Enumerable.Range(0, a.Rows - 1).Select(i => a[i, a.Rows - 1] * y[i, y.Columns - 1]).Sum();
            return newEl;
        }


        public static List<double> Reconstruction_ForRec(STAT _sample, int M, IEnumerable<int> indexes)
        {
            var (a, y) = Decomposition_ForReconstr(_sample, M, indexes);

            y = y * a.Transpose();
            double[,] newX = new double[y.Rows, y.Columns];

            int PointCount = _sample.d.Length;
            var Temp = new double[PointCount];
            int[] Counter = new int[PointCount];
            for (int j = 0; j < PointCount - M + 1; j++)
            {
                for (int i = 0; i < M; i++)
                {
                    Temp[j + i] += y[j, i];
                    Counter[i + j]++;
                }
            }
            for (int i = 0; i < PointCount; i++)
            {
                Temp[i] /= Counter[i];
            }
            return Temp.ToList();

        }

    }
    
    public class EValuesVectorResult
    {
        public double[] EValues { get; set; }       ///<Собственные числа
        public Vector[] EVectors { get; set; }     ///<Собственные векторы
    }
    
    public static class MatrixExtension
    {
        public static double Sum(this Matrix matrix, Func<double, int, int, double> func)
        {
            double sum = 0;
            for (int i = 0; i < matrix.Columns; i++)
                for (int j = 0; j < matrix.Rows; j++)
                    sum += func(matrix[i, j], i, j);

            return sum;
        }


        public static IEnumerable<double> Where(this Matrix matrix, Func<double, int, int, bool> predicate)
        {
            for (int i = 0; i < matrix.Columns; i++)
                for (int j = 0; j < matrix.Rows; j++)
                    if (predicate(matrix[i, j], i, j))
                        yield return matrix[i, j];
        }

        public static IEnumerable<double> Diagonal(this Matrix matrix)
        {
            return matrix.Where((e, i, j) => i == j);
        }

        public static IEnumerable<Vector> Vectors(this Matrix matrix)
        {
            for (int i = 0; i < matrix.Columns; i++)
            {
                yield return matrix.GetColumn(i);
            }
        }


        public static Matrix IdentityMatrix(int rows, int columns)
        {
            double[,] array = new double[rows, columns];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    array[i, j] = i == j ? 1 : 0;

            return Matrix.Create.New(array);
        }

        public static EValuesVectorResult GetEigenResult(this Matrix matrix, double eps)
        {
            var A = (Matrix)matrix.Clone();
            Matrix EigenMatrix = null;

            while (A.Sum((e, i, j) => i != j ? Math.Abs(e) : 0) > eps)
            {
                int a = 0, b = 0;
                double max = 0;

                for (int i = 0; i < A.Columns; i++)
                    for (int j = 0; j < A.Rows; j++)
                        if (j > i && Math.Abs(A[i, j]) > max)
                        {
                            a = i;
                            b = j;
                            max = Math.Abs(A[i, j]);
                        }


                double fi = 0.5 * Math.Atan(2d * A[a, b] / (A[a, a] - A[b, b])),
                    cos = Math.Cos(fi),
                    sin = Math.Sin(fi);

                Matrix U = MatrixExtension.IdentityMatrix(A.Rows, A.Columns);
                Matrix UT;
                U[a, b] = -sin;
                U[a, a] = cos;
                U[b, a] = sin;
                U[b, b] = cos;

                UT = U.Transpose();

                A = UT * A * U;
                if (EigenMatrix == null)
                    EigenMatrix = U;
                else
                    EigenMatrix = EigenMatrix * U;
            }

            return new EValuesVectorResult()
            {
                EValues = A.Diagonal().ToArray(),
                EVectors = EigenMatrix.Vectors().ToArray()
            };
        }
        
        public static Matrix GetCloneMatrix(this Matrix matrix)
        {
            var corArray = new double[matrix.Rows][];
            for (int i = 0; i < matrix.Rows; i++)
            {
                corArray[i] = new double[matrix.Columns];
                for (int j = 0; j < matrix.Columns; j++)
                {
                    corArray[i][j] = matrix[i, j];
                }
            }
            return new Matrix(corArray);
        }
    }
}