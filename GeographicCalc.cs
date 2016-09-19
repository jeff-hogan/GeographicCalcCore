using System;
using System.Collections.Generic;
using System.Linq;

namespace Dynamic.GeographicCalcService
{
    public class GeographicCalc
    {
        public void Legal2Geographic(int Township, int Range, string RangeDirection, int Section, string SubSection, int FootageNorthSouth, string FootageNorthSouthDirection, int FootageEastWest, string FootageEastWestDirection, ref double LatitudeDecimalDegrees, ref double LongitudeDecimalDegrees, ref int ErrorCode, ref string ErrorDescription)
        {
            TRSClass Location = new TRSClass();
            ErrorClass ErrorObj = new ErrorClass();
            Location.Township = Township;
            Location.Range = Range;
            Location.RangeDirection.Direction = RangeDirection;
            Location.Section = Section;
            Location.SubSection.SetSubSection(SubSection);
            Location.Footage.NorthSouthValueFeet = FootageNorthSouth;
            Location.Footage.NSDir.Direction = FootageNorthSouthDirection;
            Location.Footage.EastWestValueFeet = FootageEastWest;
            Location.Footage.EWDir.Direction = FootageEastWestDirection;

            Location = GeoCalcServiceFunctions.Legal2Geo(Location);
            LatitudeDecimalDegrees = Location.Point.Latitude;
            LongitudeDecimalDegrees = Location.Point.Longitude;
            ErrorCode = ErrorObj.Code;
            ErrorDescription = ErrorObj.Description;
            Location = null;
        }

        public void Geographic2Legal(ref int Township, ref int Range, ref string RangeDirection, ref int Section, ref string SubSection, ref int FootageNorthSouth, ref string FootageNorthSouthDirection, ref int FootageEastWest, ref string FootageEastWestDirection, double LatitudeDecimalDegrees, double LongitudeDecimalDegrees, ref int ErrorCode, ref string ErrorDescription)
        {
            ErrorClass ErrorObj = new ErrorClass();
            TRSClass Location = new TRSClass();
            Location.Point.SetLL(LatitudeDecimalDegrees, LongitudeDecimalDegrees);
            Location = GeoCalcServiceFunctions.Geo2Legal(Location);
            Township = Location.Township;
            Range = Location.Range;
            RangeDirection = Location.RangeDirection.Direction;
            Section = Location.Section;
            SubSection = Location.SubSection.ToString();
            FootageNorthSouth = Convert.ToInt32(Location.Footage.NorthSouthValueFeet);
            FootageNorthSouthDirection = Location.Footage.NSDir.Direction;
            FootageEastWest = Convert.ToInt32(Location.Footage.EastWestValueFeet);
            FootageEastWestDirection = Location.Footage.EWDir.Direction;
            ErrorCode = ErrorObj.Code;
            ErrorDescription = ErrorObj.Description;
            Location = null;
        }

        public double DegreesMinutesSeconds2DecimalDegrees(int Degrees, int Minutes, double Seconds)
        {
            //Calculate calc = new Calculate();
            return new Calculate().DegreesMinutesSeconds2DecimalDegrees(Degrees, Minutes, Seconds);
        }

        public void DecimalDegrees2DegreesMinutesSeconds(double DecimalDegrees, ref int Degrees, ref int Minutes, ref double Seconds)
        {
            //Calculate calc = new Calculate();
            new Calculate().DecimalDegrees2DegreesMinutesSeconds(DecimalDegrees, ref Degrees, ref Minutes, ref Seconds);
        }

        public string DisplayAsDegreesMinutesSeconds(double DecimalDegrees)
        {
            return DisplayAsDegreesMinutesSeconds(DecimalDegrees);
        }

        public List<double> Geographic2Utm(double Latitude, double Longitude)
        {
            PointClass point = new PointClass();
            point.SetLL(Latitude, Longitude);
            List<double> output = new List<double>();
            output.Add(point.X);
            output.Add(point.Y);
            return output;
        }
    }
}