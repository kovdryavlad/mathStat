using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Disrtibutions;

namespace Chart1._1
{
    class Modelirovanie
    {
        //15-03-2017
        public static void Exp(double lyambda, int n, string filename)
        {
            Random r = new Random();

            String[] Result = new string[n];

            for (int i = 0; i < n; i++)
            {
                var a = 1 / lyambda * Math.Log(1 / (1 - r.NextDouble()));

                Result[i] = Math.Round(a, 4).ToString();
            }

            File.WriteAllLines(filename, Result);
        }

        public static void Ravn(double a, double b, int n, string filename)
        {
            Random r = new Random();

            String[] Result = new string[n];

            for (int i = 0; i < n; i++)
            {
                double zu =a+(b-a)*r.NextDouble();

                Result[i] = Math.Round(zu, 4).ToString();
            }

            File.WriteAllLines(filename, Result);
        }

        public static void Norm(double m, double s, int n, string filename)
        {
            Random r = new Random();

            String[] Result =new string[n];

            /* Преобразование Бокса — Мюллера
            Func<double>  formula = ()=>
            {
                double sm=0;   //s method

                double y = 0;
 
                double x=0;
                do
                {
                    x= -1+2*r.NextDouble();

                    y = -1+2*r.NextDouble();

                    sm = x*x*+y*y;
                }
                while(sm>1||sm==0);

                double Sq = Math.Sqrt(-2 * Math.Log(sm) / sm);


                return m + s*x * Sq;
            };

            for (int i = 0; i < n; i++)
            {
                Result[i] = formula().ToString();
            }
             */
            
            
            for (int i = 0; i < n; i++)
            {
                double sum = 0;

                for (int j = 0; j < 12; j++)
                {
                    sum += r.NextDouble();
                }

                sum = (m + s * (sum - 6));

                Result[i] = Math.Round(sum,4).ToString();
            }
            
            File.WriteAllLines(filename, Result);
        }

        public static double[] Normik(double m, double s, int n)
        {
            Random r = new Random();

            double[] Result = new double[n];

            /* Преобразование Бокса — Мюллера
            Func<double>  formula = ()=>
            {
                double sm=0;   //s method

                double y = 0;
 
                double x=0;
                do
                {
                    x= -1+2*r.NextDouble();

                    y = -1+2*r.NextDouble();

                    sm = x*x*+y*y;
                }
                while(sm>1||sm==0);

                double Sq = Math.Sqrt(-2 * Math.Log(sm) / sm);


                return m + s*x * Sq;
            };

            for (int i = 0; i < n; i++)
            {
                Result[i] = formula().ToString();
            }
             */


            for (int i = 0; i < n; i++)
            {
                double sum = 0;

                for (int j = 0; j < 12; j++)
                {
                    sum += r.NextDouble();
                }

                sum = (m + s * (sum - 6));

                Result[i] = Math.Round(sum, 4);
            }

            return Result;
        }


        public static void ArcSin(double a, int n, string filename)
        { 
            Random r = new Random();

            String[] Result = new string[n];

            for (int i = 0; i < n; i++)
            {
                double zu = a*Math.Sin(Math.PI*(r.NextDouble()-0.5));

                Result[i] = Math.Round(zu, 4).ToString();
            }

            File.WriteAllLines(filename, Result);
        }
    }
}
