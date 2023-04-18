using System.Collections.Generic;
using System;
using System.Linq;

namespace TeltonikaTask
{
    public class DrawHistograms
    {        
        public void DrawVerticalDiagram(List<GpsData> gpsData)
        {
            Console.WriteLine("Histogram of sattelites data");
            var dataArr = FillSattelitesArr(gpsData);
            for (int i = dataArr.Max(); i > 0; i -= 200)
            {
                for (int j = 0; j < dataArr.Length; j++)
                {
                    if (dataArr[j] >= i)
                    {
                        Console.Write("\u2588  ");
                    }                    
                    else
                    {
                        Console.Write("   ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20    The highest value is {dataArr.Max()}");
        }
        
        public void DrawHorizontalChart(List<GpsData> dataArr)
        {
            Console.WriteLine();
            var speedArr = FillSpeedArr(dataArr);
            int lower = 0;
            int upper = 9;
            Console.WriteLine($" Speed  histogram {new string('-', 77)}| hits");
            for (int i = 0; i < speedArr.Length; i++)
            {
                Console.Write($"[  {lower}  -  {upper}]  ".PadRight(18) + "| ");
                if (speedArr[i] > 0)
                {
                    Console.Write("\u2591".PadLeft(speedArr[i] / 200, '\u2591').PadRight(75));
                }
                else
                {
                    Console.WriteLine($"{'|'} {speedArr[i]}".PadLeft(78));
                    continue;
                }
                Console.WriteLine($"{'|'} {speedArr[i]}");
                lower += 10;
                upper += 10;
            }
        }
        int[] FillSattelitesArr(List<GpsData> gpsData)
        {
            var calculate = new Calculate();
            int[] dataArr = new int[21];

            for (int i = 0; i < dataArr.Length; i++)
            {
                dataArr[i] = calculate.CountSattelites(i, gpsData);
            }

            return dataArr;
        }

        int[] FillSpeedArr(List<GpsData> data)
        {
            var calculate = new Calculate();
            int first = 0;
            int second = 9;
            int[] speedArr = new int[15];
            for (int i = 0; i < 15; i++)
            {
                speedArr[i] = calculate.CountSpeed(first, second, data);
                first += 10;
                second += 10;
            }

            return speedArr;
        }
    }
}