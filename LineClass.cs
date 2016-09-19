using System;

namespace Dynamic.GeographicCalcService
{
    public class LineClass
    {
        //LineClass stores the line in y-intercept form
        public double M; // Stores the slope
        public double b; // Stores the y intercept

        /// <summary>
        /// Return a point on the line at value x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public PointClass CPoint(double x)
        {
            PointClass temp = new PointClass();
            temp.X = x;
            temp.Y = x * M + b;
            return temp;
        }

        /// <summary>
        /// Return the value moved N meters along the line
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        public double Adjustment(double N)
        {
            if (N < 1.0)
            {
                return 0.0;
            }
            else
            {
                return N / Math.Sqrt(1.0 + M * M);
            }
        }

        /// <summary>
        /// Create a line from two points
        /// </summary>
        /// <param name="One"></param>
        /// <param name="Two"></param>
        public void CreateFromPoints(PointClass One, PointClass Two)
        {
            if (Two.X == One.X)
            {
                M = (Two.Y - One.Y) / 0.00000000001;
            }
            else
            {
                M = (Two.Y - One.Y) / (Two.X - One.X);
            }
            b = One.Y - M * One.X;
        }

        /// <summary>
        /// Create a line from Slope and a point
        /// </summary>
        /// <param name="Slope"></param>
        /// <param name="P"></param>
        public void CreateSlopePoint(double Slope, PointClass P)
        {
            M = Slope;
            b = P.Y - M * P.X;
        }

        /// <summary>
        /// Return the right angle slope
        /// </summary>
        /// <returns></returns>
        public double SlopAtRightAngle()
        {
            return 1.0 / M;
        }

        /// <summary>
        /// REturn Radians
        /// </summary>
        /// <returns></returns>
        public double Rdegrees()
        {
            return MeasureFunctions.InvSin(M / Math.Sqrt(1.0 + (M * M)));
        }

        /// <summary>
        /// Return a point on the line at the value y
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public PointClass CpointXfromY(double y)
        {
            PointClass temp = new PointClass();
            temp.Y = y;
            temp.X = (y - b) / M;
            return temp;
        }

    }
}