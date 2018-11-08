using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stat2
{
    class ConvertException : Exception
    {
        public List<ReadingError> errorLst;
        public ConvertException(List<ReadingError> ErrorList)
        {
            errorLst = ErrorList;
        }
    }

    class ReadingError
    {
        int row;
        int column;

        public ReadingError(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }


    public class DoubleConverter
    {
        public static double[][] ConvertToDoubleValuesInColumns(List<string[]> rows)
        {
            var maxColumn = rows.Max(r => r.Length);

            int[] asrr = new int[maxColumn];

            for (int i = 0; i < maxColumn; i++)
                asrr[i] = i;
            
            return ConvertToDoubleValuesInColumns(rows, asrr);
        }

        public static double[][] ConvertToDoubleValuesInColumns(List<string[]> rows, int[] columns)
        {
            List<ReadingError> errorLst = new List<ReadingError>();

            var resRows = rows.Count;
            var resCols = columns.Length;

            double[][] result = new double[resRows][];
            for (int i = 0; i < resRows; i++)
                result[i] = new double[resCols];

            //идет построчно
            for (int i = 0; i < rows.Count; i++)
            {
                //идет по указаным столбцам
                for (int j = 0; j < columns.Length; j++)
                {
                    try
                    {
                        result[i][j] = Convert.ToDouble(rows[i][columns[j]].Replace('.', ','));
                    }
                    catch
                    {
                        errorLst.Add(new ReadingError(i, columns[j]));
                    }
                }
            }

            if (errorLst.Count > 0)
                throw new ConvertException(errorLst);

            return result;
        }

        public static double[][] RegroupBySamples(double[][] Rows)
        {
            var RowsCount = Rows.GetLength(0);
            var ColsCount = Rows[1].Length;

            double[][] Regroupped = new double[ColsCount][];
            for (int i = 0; i < ColsCount; i++)
                Regroupped[i] = new double[RowsCount];

            for (int i = 0; i < RowsCount; i++)
                for (int j = 0; j < ColsCount; j++)
                    Regroupped[j][i] = Rows[i][j];

            return Regroupped;
        }
    }
}
