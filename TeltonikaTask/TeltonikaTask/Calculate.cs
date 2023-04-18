using System.Collections.Generic;
using System.Linq;

namespace TeltonikaTask
{
    public class Calculate
    {
        public int CountSpeed(int a, int b, List<GpsData> data)
        {
            return data.Count(x => x.Speed >= a && x.Speed <= b);
        }
        public int CountSattelites(int a, List<GpsData> data)
        {
            return data.Count(x => x.Satellites == a);
        }
    }
}