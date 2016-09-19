using System;

namespace Dynamic.GeographicCalcService
{
    public static class MeasureFunctions
    {
        const double MetersPerFeet = 1200.0 / 3937.0; //Used to convert between meters and feet
        const double Mile = 5280.0; //Feet per mile

        /// <summary>
        /// Convert Meters to Feet
        /// </summary>
        /// <param name="MetersValue"></param>
        /// <returns></returns>
        public static double Meters2Feet(double MetersValue)
        {
            return (MetersValue / MetersPerFeet);
        }

        /// <summary>
        /// Convert feet to meters
        /// </summary>
        /// <param name="FeetValue"></param>
        /// <returns></returns>
        public static double Feet2Meters(double FeetValue)
        {
            return (FeetValue * MetersPerFeet);
        }

        /// <summary>
        /// Return Half Mile in Meters
        /// </summary>
        /// <returns></returns>
        public static double HalfMile
        {
            get
            {
                return Feet2Meters(Mile / 2.0);
            }
        }

        /// <summary>
        /// Return ThreeQuarterMile in meters
        /// </summary>
        /// <returns></returns>
        public static double ThreeQuarterMile
        {
            get
            {
                return Feet2Meters(3.0 * Mile / 4.0);
            }
        }

        /// <summary>
        /// Return QuarterMile in meters
        /// </summary>
        /// <returns></returns>
        public static double QuarterMile
        {
            get
            {
                return Feet2Meters(Mile / 4.0);
            }
        }

        /// <summary>
        /// Return EighthMile in meters
        /// </summary>
        /// <returns></returns>
        public static double EighthMile
        {
            get
            {
                return Feet2Meters(Mile / 8.0);
            }
        }

        /// <summary>
        /// Find the point where two lines intersect.
        /// </summary>
        /// <param name="One"></param>
        /// <param name="Two"></param>
        /// <returns></returns>
        public static PointClass LineIntercection(LineClass One, LineClass Two)
        {
            PointClass temp = new PointClass();
            temp.X = ((Two.b - One.b) / (One.M - Two.M));
            temp = One.CPoint(temp.X);
            return temp;
        }

        /// <summary>
        /// Calculate the InverseSin for a number because C# does not have a built in function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double InvSin(double x)
        {
            return Math.Atan(x / Math.Sqrt(-x * x + 1.0));
        }

        /// <summary>
        /// Calculate the distance between two points
        /// </summary>
        /// <param name="One"></param>
        /// <param name="Two"></param>
        /// <returns></returns>
        public static double DistanceBetween(PointClass One, PointClass Two)
        {
            return Math.Sqrt((One.X - Two.X) * (One.X - Two.X)
                            + (One.Y - Two.Y) * (One.Y - Two.Y));
        }

    }
}