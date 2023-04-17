using Geolocation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TeltonikaTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<GpsData> GpsData = new List<GpsData>();
            using (StreamReader r = new StreamReader("2019-07.json"))
            {
                string json = r.ReadToEnd();
                var data = JsonConvert.DeserializeObject<List<GpsData>>(json);

                for (int i = 0; i < 1; i++)
                {
                    Console.WriteLine("Latitude: {0}", data[i].Latitude);
                    Console.WriteLine("Longitude: {0}", data[i].Longitude);
                    Console.WriteLine("GPS Time: {0}", data[i].GpsTime);
                    Console.WriteLine("Speed: {0}", data[i].Speed);
                    Console.WriteLine("Angle: {0}", data[i].Angle);
                    Console.WriteLine("Altitude: {0}", data[i].Altitude);
                    Console.WriteLine("Satellites: {0}", data[i].Satellites);
                }
                var count = data.Count(speed => speed.Speed >= 125 && speed.Speed <= 150);
                Console.WriteLine(count);
                GpsData.AddRange(data);
            }

            int CountSpeed(int a, int b, List<GpsData> data)
            {
                return data.Count(x => x.Satellites >= a && x.Satellites <= b);
            }
            int CountSattelites(int a, List<GpsData> data)
            {
                return data.Count(x => x.Satellites == a);
            }
            //Console.WriteLine($"count of 0 sat is {counting(0,GpsData)}");

            using (StreamReader r = new StreamReader("2019-08.csv"))
            {
                List<GpsData> data = new List<GpsData>();
                GpsData datagps = new GpsData();
                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    var values = line.Split(',');

                    try
                    {
                        GpsData.Add(new GpsData
                        {
                            Latitude = double.Parse(values[0], CultureInfo.InvariantCulture),   //suderinam kulturas, kad butu vienodos
                            Longitude = double.Parse(values[1], CultureInfo.InvariantCulture),
                            GpsTime = DateTime.Parse(values[2]),
                            Speed = int.Parse(values[3]),
                            Angle = int.Parse(values[4]),
                            Altitude = int.Parse(values[5]),
                            Satellites = int.Parse(values[6])

                        });
                        /*
                        Console.WriteLine(values[0]);
                        Console.WriteLine(values[1]);
                        Console.WriteLine(values[2]);
                        Console.WriteLine(values[3]);
                        Console.WriteLine(values[4]);
                        Console.WriteLine(values[5]);
                        Console.WriteLine(values[6]);*/

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line '{line}': {ex.Message}");
                    }
                }
                Console.WriteLine($"GpsData.Count is {GpsData.Count}");
            }
            //Console.WriteLine($"count of 0 sat is {counting(22, GpsData)}");

            using (FileStream fs = new FileStream("2019-09.bin", FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        /*
                        var data = reader.ReadUInt32();

                        // Convert to big-endian
                        var bigEndianData = BitConverter.GetBytes(data);
                        Array.Reverse(bigEndianData);


                        Console.WriteLine(reader.BaseStream.Position);
                        var a = reader.ReadBytes(4);
                        */

                        //var c = BitConverter.ToInt32(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt32()))); 
                        var c = BitConverter.ToInt32(ReverseBytes(reader.ReadBytes(4))) / 10000000.0;
                        //Console.WriteLine($"Latitude : {c }");
                        var d = BitConverter.ToInt32(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt32()))) / 10000000.0;
                        //Console.WriteLine($"Longitude : {d / 10000000.0}");
                        var e = BitConverter.ToInt64(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt64())));
                        var gpsTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(e);
                        //Console.WriteLine($"GPS Time: {gpsTime}");
                        var f = BitConverter.ToInt16(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt16())));
                        //Console.WriteLine($"Speed: {f}");
                        var g = BitConverter.ToInt16(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt16())));
                        //Console.WriteLine($"Angle: {g}");
                        var h = BitConverter.ToInt16(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt16())));
                        //Console.WriteLine($"Altitude: {h}");
                        var j = reader.ReadByte();
                        //Console.WriteLine($"Satellites: {j} \n");

                        GpsData.Add(new GpsData
                        {
                            Latitude = c,
                            Longitude = d,
                            GpsTime = gpsTime,
                            Speed = f,
                            Angle = g,
                            Altitude = h,
                            Satellites = j

                        });
                    }
                    /*
                    // Convert to big-endian 
                    //var bigEndianData = BitConverter.GetBytes(data);
                    //Array.Reverse(bigEndianData);
                    //int value = BitConverter.ToInt32(bigEndianData);
                    //Console.WriteLine(value/ 10000000);
                    var b =BitConverter.ToInt32(ReverseBytes(reader.ReadBytes(4))) / 10000000.0;
                    Console.WriteLine(b);
                    var latitude = Convert.ToDouble(ReverseBytes(reader.ReadBytes(4)));      //neigiamas skaicius jei pietuose platuma ilguma yra
                    int longitude = reader.ReadInt32();
                    //double gpsTime = reader.ReadDouble();
                    short speed = reader.ReadInt16();
                    short angle = reader.ReadInt16();
                    short altitude = reader.ReadInt16();
                    byte satellites = reader.ReadByte();
                    /*
                    if (gpsTime < -922337203685477 || gpsTime > 922337203685477)
                    {
                        // Handle out-of-range value
                        Console.WriteLine("GPS time value out of range: {0}", gpsTime);
                        continue;
                    }
                    //var gpsTimes = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Math.Abs(gpsTime));
                    //double actualLatitude = Math.Abs(longitude / 10000000);
                    //Console.WriteLine(latitude );


                    */
                }
            }
            Console.WriteLine($"final count {GpsData.Count}");
            Console.WriteLine($"count of 0 sat is {CountSpeed(0, 2, GpsData)}");
            static byte[] ReverseBytes(byte[] bytes)
            {
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);

                return bytes;
            }

            int[] dataArr = new int[21];
            dataArr[0] = CountSattelites(0, GpsData);
            dataArr[1] = CountSattelites(1, GpsData);
            Console.WriteLine($"1 yra {dataArr[1]}");
            dataArr[2] = CountSattelites(2, GpsData);
            Console.WriteLine($"2 yra {dataArr[2]}");
            dataArr[3] = CountSattelites(3, GpsData);
            Console.WriteLine($"3 yra {dataArr[3]}");
            dataArr[4] = CountSattelites(4, GpsData);
            dataArr[5] = CountSattelites(5, GpsData);
            dataArr[6] = CountSattelites(6, GpsData);
            dataArr[7] = CountSattelites(7, GpsData);
            dataArr[8] = CountSattelites(8, GpsData);
            dataArr[9] = CountSattelites(9, GpsData);
            dataArr[10] = CountSattelites(10, GpsData);
            dataArr[11] = CountSattelites(11, GpsData);
            dataArr[12] = CountSattelites(12, GpsData);
            dataArr[13] = CountSattelites(13, GpsData);
            dataArr[14] = CountSattelites(14, GpsData);
            dataArr[15] = CountSattelites(15, GpsData);
            dataArr[16] = CountSattelites(16, GpsData);
            dataArr[17] = CountSattelites(17, GpsData);
            dataArr[18] = CountSattelites(18, GpsData);
            dataArr[19] = CountSattelites(19, GpsData);
            dataArr[20] = CountSattelites(20, GpsData);


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

            var z = Geolocation.GeoCalculator.GetDistance(GpsData[57000].Latitude, GpsData[57000].Longitude, GpsData[57001].Latitude, GpsData[57001].Longitude, 3, DistanceUnit.Kilometers);
            //var z = Geolocation.GeoCalculator.GetDistance(GpsData[57000].Latitude, GpsData[57000].Longitude, GpsData[57001].Latitude, GpsData[57001].Longitude, 3, DistanceUnit.Kilometers);
            Console.WriteLine(z);
            TimeSpan shortestTime = TimeSpan.FromSeconds(500000);
            double bestRoadDistance = 0;
            double distance = 0;
            Queue<GpsData> bestRoad = new Queue<GpsData>();
            Queue<GpsData> roadList = new Queue<GpsData>();
            double tr = 0;
            for (int i = 0; i < GpsData.Count - 1; i++)
            {

                var j = GeoCalculator.GetDistance(GpsData[i].Latitude, GpsData[i].Longitude, GpsData[i + 1].Latitude, GpsData[i + 1].Longitude, 3, DistanceUnit.Kilometers);
                distance += j;
                tr += j;
                roadList.Enqueue(GpsData[i]);



                if (distance >= 100)
                {
                    roadList.Enqueue(GpsData[i + 1]);
                    var timeBetweenDates = roadList.ElementAt(roadList.Count - 1).GpsTime - roadList.Peek().GpsTime;

                    if (timeBetweenDates < shortestTime)
                    {
                        bestRoad = new Queue<GpsData>(roadList);
                        bestRoadDistance = distance;
                        shortestTime = timeBetweenDates;
                    }
                    var first = roadList.Dequeue();
                    distance -= GeoCalculator.GetDistance(first.Latitude, first.Longitude, roadList.First().Latitude, roadList.First().Longitude, 3, DistanceUnit.Kilometers);
                }
            }


            Console.WriteLine($"co yra {bestRoad.Count}");
            Console.WriteLine($"{bestRoad.Peek().Latitude}  and {bestRoad.ElementAt(bestRoad.Count - 1).Latitude} ");
            Console.WriteLine($"{bestRoad.Peek().Longitude}  and {bestRoad.ElementAt(bestRoad.Count - 1).Longitude} ");
            Console.WriteLine($"{bestRoad.Peek().GpsTime}  and {bestRoad.ElementAt(bestRoad.Count - 1).GpsTime} ");
            Console.WriteLine($"{bestRoad.Peek().Speed}  and {bestRoad.ElementAt(bestRoad.Count - 1).Speed} ");
            Console.WriteLine($"{bestRoad.Peek().Angle}  and {bestRoad.ElementAt(bestRoad.Count - 1).Angle} ");
            Console.WriteLine($"{bestRoad.Peek().Altitude}  and {bestRoad.ElementAt(bestRoad.Count - 1).Altitude} ");
            Console.WriteLine($"{bestRoad.Peek().Satellites}  and {bestRoad.ElementAt(bestRoad.Count - 1).Satellites} ");
            Console.WriteLine(shortestTime);
            Console.WriteLine(bestRoad.Count);
            Console.WriteLine($"Fastest road section of at least 100 km was driven over {shortestTime.TotalSeconds}s and was {bestRoadDistance:0.000}km long");
            Console.WriteLine($"Start position {bestRoad.Peek().Latitude}; {bestRoad.Peek().Longitude}");
            Console.WriteLine($"Start gps time {bestRoad.Peek().GpsTime}");
            Console.WriteLine($"End position {bestRoad.ElementAt(bestRoad.Count - 1).Latitude}; {bestRoad.ElementAt(bestRoad.Count - 1).Longitude}");
            Console.WriteLine($"End gps time {bestRoad.ElementAt(bestRoad.Count - 1).GpsTime}");

            double averageSpeed = bestRoad.Average(x => x.Speed);
            Console.WriteLine($"Average speed {averageSpeed}");




        }
    }
    public class GpsData
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public DateTime GpsTime { get; set; }
            public int Speed { get; set; }
            public int Angle { get; set; }
            public int Altitude { get; set; }
            public int Satellites { get; set; }
        }
    
}
