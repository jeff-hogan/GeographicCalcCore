
namespace Dynamic.GeographicCalcService
{
    public class FootageClass
    {
        // Values in Meters
        private double NSValue;
        public DirectionClass NSDir = new DirectionClass();
        private double EWValue;
        public DirectionClass EWDir = new DirectionClass();

        public double NorthSouthValueFeet
        {
            get
            {
                return MeasureFunctions.Meters2Feet(NSValue);
            }
            set
            {
                NSValue = MeasureFunctions.Feet2Meters(value);
            }
        }

        public double NorthSouthValueMeters
        {
            get
            {
                return NSValue;
            }
            set
            {
                NSValue = value;
            }
        }

        public double EastWestValueFeet
        {
            get
            {
                return MeasureFunctions.Meters2Feet(EWValue);
            }
            set
            {
                EWValue = MeasureFunctions.Feet2Meters(value);
            }
        }

        public double EastWestValueMeters
        {
            get
            {
                return EWValue;
            }
            set
            {
                EWValue = value;
            }
        }

        public string CombindDir()
        {
            return (NSDir.Direction + EWDir.Direction).Trim();
        }

        public override string ToString()
        {
            return NSValue + NSDir.Direction + " " + EWValue + EWDir.Direction;
        }

        /// <summary>
        /// Takes footage input as feet
        /// </summary>
        /// <param name="NorthSouthValue"></param>
        /// <param name="NorthSouthDirection"></param>
        /// <param name="EastWestValue"></param>
        /// <param name="EastWestDirection"></param>
        public void SetFootageFeet(int NorthSouthValue, string NorthSouthDirection, int EastWestValue, string EastWestDirection)
        {
            NSValue = MeasureFunctions.Feet2Meters((double)NorthSouthValue);
            EWValue = MeasureFunctions.Feet2Meters((double)EastWestValue);
            NSDir.Direction = NorthSouthDirection;
            EWDir.Direction = EastWestDirection;
        }

        /// <summary>
        /// Take footage input as meters
        /// </summary>
        /// <param name="NorthSouthValue"></param>
        /// <param name="NorthSouthDirection"></param>
        /// <param name="EastWestValue"></param>
        /// <param name="EastWestDirection"></param>
        public void SetFootageMeters(int NorthSouthValue, string NorthSouthDirection, int EastWestValue, string EastWestDirection)
        {
            NSValue = (double)NorthSouthValue;
            EWValue = (double)EastWestValue;
            NSDir.Direction = NorthSouthDirection;
            EWDir.Direction = EastWestDirection;
        }

        public bool IsValid()
        {
            int temp = 0;
            if (NSDir.IsValidNorthSouth) temp++;
            if (EWDir.IsValidEastWest) temp++;
            if (NSValue >= 0) temp++;
            if (EWValue >= 0) temp++;
            if (temp == 4) return true;
            else return false;
        }

    }
}