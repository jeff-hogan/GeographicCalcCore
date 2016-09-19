using Dynamic.GeographicCalcService;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace GeographicCalcCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TRSClass location = new TRSClass();
            location.Township = 35;
            location.Range = 57;
            location.RangeDirection = new DirectionClass("W");
            location.Section = 24;
            Console.WriteLine(location);
            location =GeoCalcServiceFunctions.Legal2Geo(location);
            Console.WriteLine(location);
        }
    }
}
