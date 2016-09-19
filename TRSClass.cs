using System;

namespace Dynamic.GeographicCalcService
{
    public class TRSClass
    {
        public int Township { get; set; }
        public int Range { get; set; }
        public DirectionClass RangeDirection = new DirectionClass();
        public int Section { get; set; }
        public SubSectionClass SubSection = new SubSectionClass();
        public FootageClass Footage = new FootageClass();

        // Corners stores the four corners of the Section
        public CornersClass Corners = new CornersClass();

        // Point stores the well location
        public PointClass Point = new PointClass();

        public ErrorClass ErrorObj = new ErrorClass();

        public TRSClass Clone()
        {
            TRSClass output = new TRSClass();
            output.Township = Township;
            output.Range = Range;
            output.RangeDirection.Direction = RangeDirection.Direction;
            output.Section = Section;

            output.SubSection.SetSubSection(SubSection.ToString());

            output.Footage.SetFootageMeters(Convert.ToInt32(Footage.NorthSouthValueMeters), Footage.NSDir.Direction, Convert.ToInt32(Footage.EastWestValueMeters), Footage.EWDir.Direction);
            output.Corners.SetPoint(0, Corners.Corners[0].X, Corners.Corners[0].Y);
            output.Corners.SetPoint(1, Corners.Corners[1].X, Corners.Corners[1].Y);
            output.Corners.SetPoint(2, Corners.Corners[2].X, Corners.Corners[2].Y);
            output.Corners.SetPoint(3, Corners.Corners[3].X, Corners.Corners[3].Y);

            output.Point.SetXY(Point.X, Point.Y);
            return output;
        }


        public void LoadFromString(string input)
        {
            string[] parts = input.Split(',');

            Township = Convert.ToInt32(parts[1]);
            Range = Convert.ToInt32(parts[2]);
            RangeDirection.Direction = parts[3];
            Section = Convert.ToInt32(parts[4]);
            SubSection.SetSubSection(parts[5]);
            Footage.NorthSouthValueFeet = Convert.ToInt32(parts[6]);
            Footage.NSDir.Direction = parts[7];
            Footage.EastWestValueFeet = Convert.ToInt32(parts[8]);
            Footage.EWDir.Direction = parts[9];
        }

        public override string ToString()
        {
            string Output;
            Output = Township.ToString();
            Output += "," + Range.ToString();
            Output += "," + RangeDirection.Direction;
            Output += "," + Section;
            Output += "," + SubSection.ToString();
            Output += "," + Footage.NorthSouthValueFeet.ToString("0000");
            Output += "," + Footage.NSDir.Direction;
            Output += "," + Footage.EastWestValueFeet.ToString("0000");
            Output += "," + Footage.EWDir.Direction;
            Output += "," + Point.Latitude.ToString("00.00000000");
            Output += "," + Point.Longitude.ToString("000.00000000");
            return Output;
        }

        public bool IsValid
        {
            get
            {
                int Ret = 0;
                if (Township > 0) Ret++;
                if ((Range > 0)) Ret++;
                if (RangeDirection.IsValidEastWest) Ret++;
                if (Section > 0) Ret++;
                if (Ret == 4)
                    return true;
                else
                    return false;
            }
        }

    }
}