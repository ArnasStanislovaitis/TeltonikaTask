using System;
using System.Collections.Generic;
using System.IO;

namespace TeltonikaTask
{
    public class ReadBin
    {
        public List<GpsData> ReadBinFile(string path)
        {   
            List<GpsData> data = new List<GpsData>();
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(fs))
                    {

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            var latitude = BitConverter.ToInt32(ReverseBytes(reader.ReadBytes(4))) / 10000000.0;
                            var longitude = BitConverter.ToInt32(ReverseBytes(reader.ReadBytes(4))) / 10000000.0;
                            var timeData = BitConverter.ToInt64(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt64())));
                            var gpsTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timeData);
                            var speed = BitConverter.ToInt16(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt16())));
                            var angle = BitConverter.ToInt16(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt16())));
                            var altitude = BitConverter.ToInt16(ReverseBytes(BitConverter.GetBytes(reader.ReadUInt16())));
                            var satelites = reader.ReadByte();

                            data.Add(new GpsData
                            {
                                Latitude = latitude,
                                Longitude = longitude,
                                GpsTime = gpsTime,
                                Speed = speed,
                                Angle = angle,
                                Altitude = altitude,
                                Satellites = satelites

                            });
                        }
                    }
                }

                return data;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the binary file: {ex.Message}");

                return default;
            }
        }

        public static byte[] ReverseBytes(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
    }
}