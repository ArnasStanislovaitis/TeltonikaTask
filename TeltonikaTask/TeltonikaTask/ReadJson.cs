using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TeltonikaTask
{
    public class ReadJson
    {
        
        public List<GpsData> ReadJsonFile(string path)
        {
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    List<GpsData> gpsData = new List<GpsData>();
                    string json = r.ReadToEnd();
                    var data = JsonConvert.DeserializeObject<List<GpsData>>(json);
                    gpsData.AddRange(data);

                    return gpsData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the JSON file: {ex.Message}");

                return default;
            }
        }
    }
}