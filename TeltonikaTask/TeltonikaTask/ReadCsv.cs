using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace TeltonikaTask
{
    public class ReadCsv
    {
        public List<GpsData> ReadCsvFile(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                List<GpsData> data = new List<GpsData>();
                
                while (!r.EndOfStream)
                {
                    var line = r.ReadLine();
                    var values = line.Split(',');

                    try
                    {
                        data.Add(new GpsData
                        {
                            Latitude = double.Parse(values[0], CultureInfo.InvariantCulture),
                            Longitude = double.Parse(values[1], CultureInfo.InvariantCulture),
                            GpsTime = DateTime.Parse(values[2]),
                            Speed = int.Parse(values[3]),
                            Angle = int.Parse(values[4]),
                            Altitude = int.Parse(values[5]),
                            Satellites = int.Parse(values[6])

                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing line '{line}': {ex.Message}");
                    }
                }
                return data;
            }
        }
    }
}