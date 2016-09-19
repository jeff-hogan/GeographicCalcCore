using System;

namespace Dynamic.GeographicCalcService
{
    public class Calculate
    {
        public double DegreesMinutesSeconds2DecimalDegrees(int Degrees, int Minutes, double Seconds)
        {
            int OriginalSign = Degrees;
            if (Degrees < 0) Degrees *= -1;
            double output = (double)Degrees + ((double)Minutes / 60.0) + ((double)Seconds / 60.0 / 60.0);
            if (OriginalSign < 0)
                return -output;
            else
                return output;
        }


        public string DisplayAsDegreesMinutesSeconds(double DecimalDegrees)
        {
            int Degrees = 0;
            int Minutes = 0;
            double Seconds = 0.0;

            DecimalDegrees2DegreesMinutesSeconds(DecimalDegrees, ref Degrees, ref Minutes, ref Seconds);

            System.Text.StringBuilder output = new System.Text.StringBuilder();
            output.Append(Degrees);
            output.Append((char)176);
            output.Append(" " + Minutes + "'");
            output.Append(" " + Seconds.ToString("00.00") + "\"");
            return output.ToString();
        }

        public void DecimalDegrees2DegreesMinutesSeconds(double DecimalDegrees,
                        ref int Degrees,
                        ref int Minutes,
                        ref double Seconds)
        {
            double temp = 0.0;
            Degrees = Fix(DecimalDegrees);
            temp = DecimalDegrees - Degrees;
            temp = temp * 60;
            Minutes = Fix(temp);
            temp = temp - Minutes;
            temp = temp * 60;
            Seconds = temp;

            if (Minutes < 0) Minutes *= -1;
            if (Seconds < 0) Seconds *= -1;

            if (Seconds.ToString("00.00") == "60.00")
            {
                Seconds = 0.0;
                Minutes += 1;
            }
        }

        public int Fix(double input)
        {
            if (input > 0)
                return (int)Math.Floor(input);
            else
                return (int)Math.Ceiling(input);
        }

    }
}
