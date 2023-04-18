using System.Collections.Generic;

namespace TeltonikaTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadJson readJson= new ReadJson();
            ReadCsv readCsv = new ReadCsv();
            ReadBin readBin = new ReadBin();
            DrawHistograms drawHistograms = new DrawHistograms();
            RoadSection roadSection= new RoadSection();

            List<GpsData> GpsData = new List<GpsData>();

            var listFromJson = readJson.ReadJsonFile("2019-07.json");
            var listFromCsv = readCsv.ReadCsvFile("2019-08.csv");
            var listFromBin = readBin.ReadBinFile("2019-09.bin");

            GpsData.AddRange(listFromJson);
            GpsData.AddRange(listFromCsv);
            GpsData.AddRange(listFromBin);
            
            drawHistograms.DrawVerticalDiagram(GpsData);
            drawHistograms.DrawHorizontalChart(GpsData);

            roadSection.DisplayBestRoad(GpsData);            
        }
    }
}