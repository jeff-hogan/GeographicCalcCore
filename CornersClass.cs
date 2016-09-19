using System;

namespace Dynamic.GeographicCalcService
{
    /// <summary>
    /// Stores four points to make a rectangular section
    /// </summary>
    public class CornersClass
    {
        public PointClass[] Corners = new PointClass[4];
        public CornersClass()
        {
            //Initialize the four corners
            for (int x = 0; x < 4; x++)
            {
                Corners[x] = new PointClass();
            }
        }

        //Set one of the four corners to an utm x,y
        public void SetPoint(int Corner, double UTMX, double UTMY)
        {
            Corners[Corner].SetXY(UTMX, UTMY);
        }

        // Looks at the Four points and returns false if
        // the points are not in the correct orientation for a UTM section
        public bool IsValid()
        {
            int temp = 0;
            if (Corners[0].X != 0 && Corners[0].Y != 0) temp++;
            if (Corners[1].X != 0 && Corners[1].Y != 0) temp++;
            if (Corners[2].X != 0 && Corners[2].Y != 0) temp++;
            if (Corners[3].X != 0 && Corners[3].Y != 0) temp++;
            if (Corners[0].X > Corners[1].X) temp++;
            if (Corners[0].Y > Corners[3].Y) temp++;
            if (Corners[1].Y > Corners[2].Y) temp++;
            if (Corners[2].X < Corners[3].X) temp++;
            if (temp == 8) return true;
            else return false;
        }

        /// <summary>
        /// Returns a point at the center of the section
        /// </summary>
        /// <returns></returns>
        public PointClass Center()
        {
            PointClass point = new PointClass();

            LineClass right = new LineClass();
            LineClass bottom = new LineClass();
            LineClass second = new LineClass();
            LineClass first = new LineClass();
            PointClass temp = new PointClass();
            if (IsValid())
            {
                bottom.CreateFromPoints(Corners[2], Corners[3]);
                right.CreateFromPoints(Corners[0], Corners[3]);
                first.CreateSlopePoint(right.M, bottom.CPoint(Corners[3].X - bottom.Adjustment(MeasureFunctions.HalfMile)));

                bottom.CPoint(Corners[3].X - bottom.Adjustment(MeasureFunctions.HalfMile));
                right.CPoint(Corners[3].X + (Math.Sign(right.M) * right.Adjustment(MeasureFunctions.HalfMile)));
                second.CreateSlopePoint(bottom.M, right.CPoint(Corners[3].X + (Math.Sign(right.M) * right.Adjustment(GeographicCalcService.MeasureFunctions.HalfMile))));

                temp = MeasureFunctions.LineIntercection(first, second);
                point = temp;
            }
            else
            {
                temp.X = -1;
                temp.Y = -1;
                point = temp;
            }
            return point;
        }

        /// <summary>
        /// Returns true if point p is within the section.
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public bool IsWithIn(PointClass P)
        {
            // Accurate but slow
            // Checks To make sure the point is in the section.
            LineClass top = new LineClass();
            LineClass bottom = new LineClass();
            LineClass left = new LineClass();
            LineClass right = new LineClass();
            LineClass ATop = new LineClass();
            LineClass ABottom = new LineClass();
            LineClass ALeft = new LineClass();
            LineClass ARight = new LineClass();
            PointClass temp;
            int Test;
            top.CreateFromPoints(Corners[0], Corners[1]);
            bottom.CreateFromPoints(Corners[2], Corners[3]);
            left.CreateFromPoints(Corners[1], Corners[2]);
            right.CreateFromPoints(Corners[0], Corners[3]);
            ATop.CreateSlopePoint(top.M, P);
            ABottom.CreateSlopePoint(bottom.M, P);
            ALeft.CreateSlopePoint(left.M, P);
            ARight.CreateSlopePoint(right.M, P);
            Test = 0;
            temp = MeasureFunctions.LineIntercection(ARight, top);
            if (temp.X <= Corners[0].X) Test++;
            temp = MeasureFunctions.LineIntercection(ARight, bottom);
            if (temp.X <= Corners[3].X) Test++;
            temp = MeasureFunctions.LineIntercection(ALeft, top);
            if (temp.X >= Corners[1].X) Test++;
            temp = MeasureFunctions.LineIntercection(ALeft, bottom);
            if (temp.X >= Corners[2].X) Test++;
            temp = MeasureFunctions.LineIntercection(ATop, right);
            if (temp.Y <= Corners[0].Y) Test++;
            temp = MeasureFunctions.LineIntercection(ATop, left);
            if (temp.Y <= Corners[1].Y) Test++;
            temp = MeasureFunctions.LineIntercection(ABottom, right);
            if (temp.Y >= Corners[3].Y) Test++;
            temp = MeasureFunctions.LineIntercection(ABottom, left);
            if (temp.Y >= Corners[2].Y) Test++;

            if (Test == 8) return true;
            else return false;

        }

        /// <summary>
        /// Returns true is the point is near the section
        /// </summary>
        /// <param name="APoint"></param>
        /// <returns></returns>
        public bool IsNearSection(PointClass APoint)
        {
            ///This function is used because of rounding/conversion errors a
            ///point near a section line will show that the point is outside 
            ///of the section when IsWithIn is used but the section lines are 
            ///not accurate enough to no for sure so this function tells us
            ///if it is close to the section. If it is outside of the section
            ///and not near the section then throw and error.
            double Distance = MeasureFunctions.Feet2Meters(2.0 * 5280.0);
            if (Math.Abs((Center().X + Center().Y) - (APoint.X + APoint.Y)) > Distance)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Returns the point within the section based off of subsection information
        /// </summary>
        /// <param name="subsec"></param>
        /// <returns></returns>
        public PointClass LocateSubSection(SubSectionClass subsec)
        {
            PointClass LocateSubSectionOut = new PointClass();
            LineClass right = new LineClass();
            LineClass bottom = new LineClass();
            LineClass second = new LineClass();
            LineClass first = new LineClass();
            PointClass temp = new PointClass();
            if (IsValid())
            {
                bottom.CreateFromPoints(Corners[2], Corners[3]);
                right.CreateFromPoints(Corners[0], Corners[3]);
                first.CreateSlopePoint(right.M, bottom.CPoint((Corners[3].X - bottom.Adjustment(subsec.ROffset().X))));
                second.CreateSlopePoint(bottom.M, right.CPoint((Corners[3].X + (Math.Sign(right.M) * right.Adjustment(subsec.ROffset().Y)))));
                temp = MeasureFunctions.LineIntercection(first, second);
                LocateSubSectionOut = temp;
            }
            else
            {
                temp.X = -1;
                temp.Y = -1;
                LocateSubSectionOut = temp;
            }
            return LocateSubSectionOut;
        }

        /// <summary>
        /// Return the point within the section based off of footage information
        /// </summary>
        /// <param name="Foot"></param>
        /// <returns></returns>
        public PointClass LocateFootage(FootageClass Foot)
        {
            PointClass LocateFootageOut = new PointClass();

            LineClass right = new LineClass();
            LineClass bottom = new LineClass();
            LineClass left = new LineClass();
            LineClass top = new LineClass();
            LineClass second = new LineClass();
            LineClass first = new LineClass();
            PointClass temp = new PointClass();
            if (IsValid())
            {
                //  *** SE Corner Based of south and east borders
                if (Foot.CombindDir() == "NW")
                {
                    bottom.CreateFromPoints(Corners[2], Corners[3]);
                    right.CreateFromPoints(Corners[0], Corners[3]);
                    first.CreateSlopePoint(right.M, bottom.CPoint((Corners[3].X - bottom.Adjustment(Foot.EastWestValueMeters))));
                    second.CreateSlopePoint(bottom.M, right.CPoint((Corners[3].X + (Math.Sign(right.M) * right.Adjustment(Foot.NorthSouthValueMeters)))));
                    //  *** SW corner based of south and west borders
                }
                else if ((Foot.CombindDir() == "NE"))
                {
                    bottom.CreateFromPoints(Corners[2], Corners[3]);
                    left.CreateFromPoints(Corners[1], Corners[2]);
                    first.CreateSlopePoint(left.M, bottom.CPoint((Corners[2].X + bottom.Adjustment(Foot.EastWestValueMeters))));
                    second.CreateSlopePoint(bottom.M, left.CPoint(Corners[2].X + (Math.Sign(left.M) * left.Adjustment(Foot.NorthSouthValueMeters))));
                    //  *** NW corner based off west and north borders
                }
                else if ((Foot.CombindDir() == "SE"))
                {
                    top.CreateFromPoints(Corners[0], Corners[1]);
                    left.CreateFromPoints(Corners[1], Corners[2]);
                    first.CreateSlopePoint(left.M, top.CPoint((Corners[1].X + top.Adjustment(Foot.EastWestValueMeters))));
                    second.CreateSlopePoint(top.M, left.CPoint((Corners[1].X - (Math.Sign(left.M) * left.Adjustment(Foot.NorthSouthValueMeters)))));
                    //  *** NE corner based off east and north border
                }
                else if ((Foot.CombindDir() == "SW"))
                {
                    top.CreateFromPoints(Corners[0], Corners[1]);
                    right.CreateFromPoints(Corners[0], Corners[3]);
                    first.CreateSlopePoint(right.M, top.CPoint((Corners[0].X - bottom.Adjustment(Foot.EastWestValueMeters))));
                    second.CreateSlopePoint(top.M, right.CPoint((Corners[0].X - (Math.Sign(right.M) * right.Adjustment(Foot.NorthSouthValueMeters)))));
                }
                temp = MeasureFunctions.LineIntercection(first, second);
                LocateFootageOut = temp;
            }
            else
            {
                temp.X = -1;
                temp.Y = -1;
                LocateFootageOut = temp;
            }
            return LocateFootageOut;
        }

        public override string ToString()
        {
            string s;
            s = Corners[0].ToString() + ", ";
            s += Corners[1].ToString() + ", ";
            s += Corners[2].ToString() + ", ";
            s += Corners[3].ToString();
            return s;
        }

        /// <summary>
        /// Return the footage for the point within the section
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        public FootageClass Point2Footage(PointClass P)
        {
            FootageClass Ans = new FootageClass();
            LineClass left = new LineClass();
            LineClass right = new LineClass();
            LineClass bottom = new LineClass();
            LineClass top = new LineClass();
            LineClass hori = new LineClass();
            LineClass vert = new LineClass();
            double temp1;
            double temp2;
            double temp3;
            double temp4;
            int temp = 0;
            FootageClass tFootage = new FootageClass();
            temp1 = MeasureFunctions.DistanceBetween(Corners[0], P);
            temp2 = MeasureFunctions.DistanceBetween(Corners[1], P);
            temp3 = MeasureFunctions.DistanceBetween(Corners[2], P);
            temp4 = MeasureFunctions.DistanceBetween(Corners[3], P);
            if (temp1 < temp2 && temp1 < temp3 && temp1 < temp4)
            {
                temp = 1;
            }
            else if (temp2 < temp1 && temp2 < temp3 && temp2 < temp4)
            {
                temp = 2;
            }
            else if (temp3 < temp1 && temp3 < temp2 && temp3 < temp4)
            {
                temp = 3;
            }
            else if (temp4 < temp1 && temp4 < temp2 && temp4 < temp3)
            {
                temp = 4;
            }
            if (temp == 1)
            {
                top.CreateFromPoints(Corners[0], Corners[1]);
                right.CreateFromPoints(Corners[0], Corners[3]);
                hori.CreateSlopePoint(top.M, P);
                vert.CreateSlopePoint(right.M, P);

                int northing = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(top, vert), P));
                int easting = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(right, hori), P));
                tFootage.SetFootageMeters(northing, "S", easting, "W");
            }
            else if (temp == 2)
            {
                top.CreateFromPoints(Corners[0], Corners[1]);
                left.CreateFromPoints(Corners[1], Corners[2]);
                hori.CreateSlopePoint(top.M, P);
                vert.CreateSlopePoint(left.M, P);

                int northing = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(top, vert), P));
                int easting = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(left, hori), P));
                tFootage.SetFootageMeters(northing, "S", easting, "E");
            }
            else if (temp == 3)
            {
                bottom.CreateFromPoints(Corners[2], Corners[3]);
                left.CreateFromPoints(Corners[1], Corners[2]);
                hori.CreateSlopePoint(bottom.M, P);
                vert.CreateSlopePoint(left.M, P);

                int northing = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(bottom, vert), P));
                int easting = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(left, hori), P));
                tFootage.SetFootageMeters(northing, "N", easting, "E");
            }
            else if (temp == 4)
            {
                bottom.CreateFromPoints(Corners[2], Corners[3]);
                right.CreateFromPoints(Corners[0], Corners[3]);
                hori.CreateSlopePoint(bottom.M, P);
                vert.CreateSlopePoint(right.M, P);

                int northing = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(bottom, vert), P));
                int easting = Convert.ToInt32(MeasureFunctions.DistanceBetween(MeasureFunctions.LineIntercection(right, hori), P));
                tFootage.SetFootageMeters(northing, "N", easting, "W");
            }
            return tFootage;
        }

        /// <summary>
        /// Return the subsection for a given point within the section based off midpoints.
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public SubSectionClass Point2SubSec(PointClass Point)
        {
            SubSectionClass tSubSec = new SubSectionClass();
            PointClass Center = new PointClass();
            LineClass LineOne = new LineClass();
            LineClass LineTwo = new LineClass();
            LineClass HoriLine = new LineClass();
            LineClass VertLine = new LineClass();
            LineClass SubLineOne = new LineClass();
            LineClass SubLineTwo = new LineClass();

            LineOne.CreateFromPoints(Corners[0], Corners[2]);
            LineTwo.CreateFromPoints(Corners[1], Corners[3]);
            Center = MeasureFunctions.LineIntercection(LineOne, LineTwo);
            LineOne.CreateFromPoints(Corners[2], Corners[3]);
            LineTwo.CreateFromPoints(Corners[0], Corners[3]);
            HoriLine.CreateSlopePoint(LineOne.M, Center);
            VertLine.CreateSlopePoint(LineTwo.M, Center);
            // find first part
            if (HoriLine.CPoint(Point.X).Y >= Point.Y)
            {
                if (VertLine.CpointXfromY(Point.Y).X >= Point.X)
                {
                    tSubSec.FirstPart = "C";
                }
                else
                {
                    tSubSec.FirstPart = "D";
                }
            }
            else if (VertLine.CpointXfromY(Point.Y).X >= Point.X)
            {
                tSubSec.FirstPart = "B";
            }
            else
            {
                tSubSec.FirstPart = "A";
            }
            // find second part of Subsection
            // start by defining center point lines
            if (tSubSec.FirstPart == "A")
            {
                LineOne.CreateFromPoints(Corners[0], Corners[1]);
                SubLineOne.CreateFromPoints(Corners[0], Corners[2]);
                SubLineTwo.CreateFromPoints(MeasureFunctions.LineIntercection(VertLine, LineOne), MeasureFunctions.LineIntercection(HoriLine, LineTwo));
                Center = MeasureFunctions.LineIntercection(SubLineOne, SubLineTwo);
                LineOne.CreateFromPoints(Corners[2], Corners[3]);
                HoriLine.CreateSlopePoint(LineOne.M, Center);
                VertLine.CreateSlopePoint(LineTwo.M, Center);
            }
            else if (tSubSec.FirstPart == "B")
            {
                LineOne.CreateFromPoints(Corners[0], Corners[1]);
                LineTwo.CreateFromPoints(Corners[1], Corners[2]);
                SubLineOne.CreateFromPoints(Corners[1], Corners[3]);
                SubLineTwo.CreateFromPoints(MeasureFunctions.LineIntercection(VertLine, LineOne), MeasureFunctions.LineIntercection(HoriLine, LineTwo));
                Center = MeasureFunctions.LineIntercection(SubLineOne, SubLineTwo);
                LineOne.CreateFromPoints(Corners[2], Corners[3]);
                LineTwo.CreateFromPoints(Corners[0], Corners[3]);
                HoriLine.CreateSlopePoint(LineOne.M, Center);
                VertLine.CreateSlopePoint(LineTwo.M, Center);
            }
            else if (tSubSec.FirstPart == "C")
            {
                LineTwo.CreateFromPoints(Corners[1], Corners[2]);
                SubLineOne.CreateFromPoints(Corners[0], Corners[2]);
                SubLineTwo.CreateFromPoints(MeasureFunctions.LineIntercection(VertLine, LineOne), MeasureFunctions.LineIntercection(HoriLine, LineTwo));
                Center = MeasureFunctions.LineIntercection(SubLineOne, SubLineTwo);
                LineOne.CreateFromPoints(Corners[2], Corners[3]);
                HoriLine.CreateSlopePoint(LineOne.M, Center);
                VertLine.CreateSlopePoint(LineTwo.M, Center);
            }
            else if (tSubSec.FirstPart == "D")
            {
                SubLineOne.CreateFromPoints(Corners[1], Corners[3]);
                SubLineTwo.CreateFromPoints(MeasureFunctions.LineIntercection(VertLine, LineOne), MeasureFunctions.LineIntercection(HoriLine, LineTwo));
                Center = MeasureFunctions.LineIntercection(SubLineOne, SubLineTwo);
                HoriLine.CreateSlopePoint(LineOne.M, Center);
                VertLine.CreateSlopePoint(LineTwo.M, Center);
            }
            // sub lines found and sub centers found
            // find second part now
            if (HoriLine.CPoint(Point.X).Y >= Point.Y)
            {
                if ((VertLine.CpointXfromY(Point.Y).X >= Point.X))
                {
                    tSubSec.SecondPart = "C";
                }
                else
                {
                    tSubSec.SecondPart = "D";
                }
            }
            else if (VertLine.CpointXfromY(Point.Y).X >= Point.X)
            {
                tSubSec.SecondPart = "B";
            }
            else
            {
                tSubSec.SecondPart = "A";
            }
            return tSubSec;
        }
    }
}
