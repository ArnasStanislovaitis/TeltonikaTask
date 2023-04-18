using Geolocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TeltonikaTask
{
    public class RoadSection
    {
        public (Queue<GpsData>,double,TimeSpan) FindShortestTimeRoad(List<GpsData> gpsData)
        {
            TimeSpan shortestTime = TimeSpan.FromSeconds(500000);
            double bestRoadDistance = 0;
            double distance = 0;
            Queue<GpsData> bestRoad = new Queue<GpsData>();
            Queue<GpsData> roadList = new Queue<GpsData>();

            for (int i = 0; i < gpsData.Count - 1; i++)
            {

                var j = GeoCalculator.GetDistance(gpsData[i].Latitude, gpsData[i].Longitude, gpsData[i + 1].Latitude, gpsData[i + 1].Longitude, 3, DistanceUnit.Kilometers);
                distance += j;
                roadList.Enqueue(gpsData[i]);

                if (distance >= 100)
                {
                    roadList.Enqueue(gpsData[i + 1]);
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

            return (bestRoad,bestRoadDistance,shortestTime);
        }

        public void DisplayBestRoad(List<GpsData> gpsData)
        {
            var results = FindShortestTimeRoad(gpsData);
            var bestRoad = results.Item1;
            var bestRoadDistance = results.Item2;
            var shortestTime = results.Item3;
            Console.WriteLine("\n");
            Console.WriteLine($"Fastest road section of at least 100 km was driven over {shortestTime.TotalSeconds}s and was {bestRoadDistance:0.000}km long");
            Console.WriteLine($"Start position {bestRoad.Peek().Latitude}; {bestRoad.Peek().Longitude}");
            Console.WriteLine($"Start gps time {bestRoad.Peek().GpsTime}");
            Console.WriteLine($"End position {bestRoad.ElementAt(bestRoad.Count - 1).Latitude}; {bestRoad.ElementAt(bestRoad.Count - 1).Longitude}");
            Console.WriteLine($"End gps time {bestRoad.ElementAt(bestRoad.Count - 1).GpsTime}");

            double averageSpeed = bestRoad.Average(x => x.Speed);
            Console.WriteLine($"Average speed {Math.Round(averageSpeed,2)}km/h"); 
        }
    }
}