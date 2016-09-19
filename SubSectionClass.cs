using System.Linq;

namespace Dynamic.GeographicCalcService
{
    public class SubSectionClass
    {
        /// <summary>
        /// Class contains the subsection information.
        /// It is stored in ABCD format but accept ether format.
        /// #########
        /// # B # A #
        /// #########
        /// # C # D #
        /// #########
        /// </summary>
        public string FirstPart { get; set; }
        public string SecondPart { get; set; }

        public void SetSubSection(string SubSection)
        {
            SubSection = SubSection.ToUpper();
            if (SubSection.Contains('A') || SubSection.Contains('B')
                || SubSection.Contains('C') || SubSection.Contains('D'))
            {
                SetFromABCD(SubSection);
            }
            else
            {
                SetFromDir(SubSection);
            }
        }

        private void SetFromABCD(string x)
        {
            x = x.ToUpper();
            if (x.Length == 1)
            {
                FirstPart = x.Substring(0, 1);
                SecondPart = "O";
            }
            else
            {
                FirstPart = x.Substring(0, 1);
                SecondPart = x.Substring(x.Length - 1);
            }
        }

        /// <summary>
        /// Convert NESW format to the ABCD format for one subsection
        /// </summary>
        /// <param name="subsection"></param>
        /// <returns></returns>
        private string LongToShort(string subsection)
        {
            string output = string.Empty;
            switch (subsection.ToUpper())
            {
                case "NE": output = "A"; break;
                case "NW": output = "B"; break;
                case "SW": output = "C"; break;
                case "SE": output = "D"; break;
                case "OO": output = "O"; break;
                default: output = ""; break;
            }

            return output;
        }

        /// <summary>
        /// Convert ABCD format to the NESW format for one subsection
        /// </summary>
        /// <param name="subsection"></param>
        /// <returns></returns>
        private string ShortToLong(string subsection)
        {
            string output = string.Empty;
            switch (subsection.ToUpper())
            {
                case "A": output = "NE"; break;
                case "B": output = "NW"; break;
                case "C": output = "SW"; break;
                case "D": output = "SE"; break;
                case "O": output = "OO"; break;
                default: output = ""; break;
            }

            return output;
        }

        private void SetFromDir(string subsection)
        {
            FirstPart = string.Empty;
            SecondPart = string.Empty;
            if (subsection.Length == 4)
            {
                FirstPart = LongToShort(subsection.Substring(2, 2));
                SecondPart = LongToShort(subsection.Substring(0, 2));
            }
            else if (subsection.Length == 2)
            {
                FirstPart = LongToShort(subsection);
                SecondPart = "O";
            }
        }

        public override string ToString()
        {
            return FirstPart + SecondPart;
        }

        public string ToLongString()
        {
            return ShortToLong(SecondPart) + ShortToLong(FirstPart);
        }

        public bool IsValid()
        {
            int temp = 0;
            if (FirstPart == "A" || FirstPart == "B" || FirstPart == "C"
                        || FirstPart == "D" || FirstPart == "O")
            {
                temp = temp + 1;
            }
            if (SecondPart == "A" || SecondPart == "B" || SecondPart == "C"
                        || SecondPart == "D" || SecondPart == "O")
            {
                temp = temp + 1;
            }
            if (temp == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculate the offset within the section to the location in meters.
        /// </summary>
        /// <returns></returns>
        public PointClass ROffset()
        {
            ///This is just a relative calculation
            ///It is a section with (0,0) at the bottom right corner.
            ///x increases to the left and y increase up
            PointClass ROffsetOut = new PointClass();
            if (FirstPart == "A")
            {
                ROffsetOut.X = MeasureFunctions.QuarterMile;
                ROffsetOut.Y = MeasureFunctions.ThreeQuarterMile;
            }
            else if (FirstPart == "B")
            {
                ROffsetOut.X = MeasureFunctions.ThreeQuarterMile;
                ROffsetOut.Y = MeasureFunctions.ThreeQuarterMile;
            }
            else if (FirstPart == "C")
            {
                ROffsetOut.X = MeasureFunctions.ThreeQuarterMile;
                ROffsetOut.Y = MeasureFunctions.QuarterMile;
            }
            else if (FirstPart == "D")
            {
                ROffsetOut.X = MeasureFunctions.QuarterMile;
                ROffsetOut.Y = MeasureFunctions.QuarterMile;
            }
            else if (FirstPart == "O")
            {
                ROffsetOut.X = MeasureFunctions.HalfMile;
                ROffsetOut.Y = MeasureFunctions.HalfMile;
            }

            if (SecondPart == "A")
            {
                ROffsetOut.X -= MeasureFunctions.EighthMile;
                ROffsetOut.Y += MeasureFunctions.EighthMile;
            }
            else if (SecondPart == "B")
            {
                ROffsetOut.X += MeasureFunctions.EighthMile;
                ROffsetOut.Y += MeasureFunctions.EighthMile;
            }
            else if (SecondPart == "C")
            {
                ROffsetOut.X += MeasureFunctions.EighthMile;
                ROffsetOut.Y -= MeasureFunctions.EighthMile;
            }
            else if (SecondPart == "D")
            {
                ROffsetOut.X -= MeasureFunctions.EighthMile;
                ROffsetOut.Y -= MeasureFunctions.EighthMile;
            }
            else if (SecondPart == "O")
            {
                //Do nothing already in proper location.
            }

            return ROffsetOut;
        }

    }
}