using System;

namespace Dynamic.GeographicCalcService
{
    public class PointClass
    {
        //X and Y are UTM coordinates
        public double X { get; set; }
        public double Y { get; set; }

        public PointClass()
        {
        }

        public PointClass(double UTMX, double UTMY)
        {
            SetXY(UTMX, UTMY);
        }

        public PointClass(int LatitudeDegrees, int LatitudeMinutes, double LatitudeSeconds, int LongitudeDegrees, int LongitudeMinutes, double LongitudeSeconds)
        {
            SetXY(LatitudeDegrees, LatitudeMinutes, LatitudeSeconds, LongitudeDegrees, LongitudeMinutes, LongitudeSeconds);
        }

        public void SetXY(double UTMX, double UTMY)
        {
            X = UTMX;
            Y = UTMY;
        }

        public void SetLL(double LatitudeDecimalDegrees, double LongitudeDemimalDegrees)
        {
            double[] ret = LatLong2UTM(LatitudeDecimalDegrees, LongitudeDemimalDegrees);
            this.X = ret[0];
            this.Y = ret[1];
        }

        public void SetXY(int LatitudeDegrees, int LatitudeMinutes, double LatitudeSeconds, int LongitudeDegrees, int LongitudeMinutes, double LongitudeSeconds)
        {
            SetLL(ConvertDegreeMinutesSeconds2DecimalDegrees(LatitudeDegrees, LatitudeMinutes, LatitudeSeconds), ConvertDegreeMinutesSeconds2DecimalDegrees(LongitudeDegrees, LongitudeMinutes, LongitudeSeconds));
        }

        public double Northing
        {
            get
            {
                return Y;
            }
        }

        public double Easting
        {
            get
            {
                return X;
            }
        }

        public double Latitude
        {
            get
            {
                if (this.IsValid)
                {
                    double[] ret = UTM2LatLong(X, Y);
                    return ret[0];
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Longitude
        {
            get
            {
                if (this.IsValid)
                {
                    double[] ret = UTM2LatLong(X, Y);
                    return ret[1];
                }
                else
                {
                    return 0;
                }
            }
        }

        public override string ToString()
        {
            return "(" + Latitude + "," + Longitude + ")";
        }

        public bool IsValid
        {
            get
            {
                if (X > 0 & Y > 0) return true;
                else return false;
            }
        }

        public double ConvertDegreeMinutesSeconds2DecimalDegrees(int Degrees, int Minutes, double Seconds)
        {
            return Degrees + (Minutes + Seconds / 60.0) / 60.0;
        }


        public void ConvertDecimalDegrees2DegreesMinutesSeconds(double DecimalDegrees, ref int Degrees, ref int Minutes, ref double Seconds)
        {
            double temp = 0.0;
            Degrees = (int)Math.Truncate(DecimalDegrees);
            temp = DecimalDegrees - Degrees;
            temp = temp * 60.0;
            Minutes = (int)Math.Truncate(temp);
            temp = temp - Minutes;
            temp = temp * 60.0;
            Seconds = temp;
        }

        private double[] UTM2LatLong(double Easting, double Northing)
        {
            double pi = 0;
            double FOURTHPI = 0;
            double deg2rad = 0;
            double rad2deg = 0;

            double k0 = 0;
            double A = 0;
            double eccSquared = 0;
            double eccPrimeSquared = 0;
            double e1 = 0;
            double N1 = 0;
            double T1 = 0;
            double C1 = 0;
            double R1 = 0;
            double d = 0;
            double M = 0;
            double LongOrigin = 0;
            double mu = 0;
            double phi1 = 0;
            double phi1rad = 0;
            double x = 0;
            double y = 0;
            int ZoneNumber = 0;
            //string ZoneLetter = null;
            //int NorthernHemisphere = 0;

            pi = 3.14159265;
            FOURTHPI = pi / 4.0;
            deg2rad = pi / 180.0;
            rad2deg = 180.0 / pi;

            k0 = 0.9996;
            A = 6378137.0;
            eccSquared = 0.00669438;
            e1 = (1.0 - Math.Sqrt(1.0 - eccSquared)) / (1.0 + Math.Sqrt(1.0 - eccSquared));

            x = Easting - 500000.0;
            y = Northing;

            ZoneNumber = 14;
            LongOrigin = (ZoneNumber - 1.0) * 6.0 - 180.0 + 3.0;
            eccPrimeSquared = (eccSquared) / (1.0 - eccSquared);
            M = y / k0;
            mu = M / (A * (1.0 - (eccSquared / 4.0) - (3.0 * eccSquared * eccSquared / 64.0) - (5.0 * eccSquared * eccSquared * eccSquared / 256.0)));
            phi1rad = mu + (3.0 * e1 / 2.0 - 27.0 * e1 * e1 * e1 / 32.0) * Math.Sin(2.0 * mu) + (21.0 * e1 * e1 / 16.0 - 55.0 * e1 * e1 * e1 * e1 / 32.0) * Math.Sin(4.0 * mu) + (151.0 * e1 * e1 * e1 / 96.0) * Math.Sin(6.0 * mu);
            phi1 = phi1rad * rad2deg;
            N1 = A / Math.Sqrt(1.0 - eccSquared * Math.Sin(phi1rad) * Math.Sin(phi1rad));
            T1 = Math.Tan(phi1rad) * Math.Tan(phi1rad);
            C1 = eccPrimeSquared * Math.Cos(phi1rad) * Math.Cos(phi1rad);
            R1 = A * (1.0 - eccSquared) / Math.Pow(1.0 - eccSquared * Math.Sin(phi1rad) * Math.Sin(phi1rad), 1.5);

            d = x / (N1 * k0);
            double Latitude = phi1rad - (N1 * Math.Tan(phi1rad) / R1) * (d * d / 2.0 - (5.0 + 3.0 * T1 + 10.0 * C1 - 4.0 * C1 * C1 - 9.0 * eccPrimeSquared) * d * d * d * d / 24.0 + (61.0 + 90.0 * T1 + 298.0 * C1 + 45.0 * T1 * T1 - 252.0 * eccPrimeSquared - 3.0 * C1 * C1) * d * d * d * d * d * d / 720.0);

            Latitude = Latitude * rad2deg;

            double Longitude = (d - (1.0 + 2.0 * T1 + C1) * d * d * d / 6.0 + (5.0 - 2.0 * C1 + 28.0 * T1 - 3.0 * C1 * C1 + 8.0 * eccPrimeSquared + 24.0 * T1 * T1) * d * d * d * d * d / 120.0) / Math.Cos(phi1rad);
            Longitude = LongOrigin + Longitude * rad2deg;

            double[] ret = new double[] { Latitude, Longitude };
            return ret;
        }


        private double[] LatLong2UTM(double Latitude, double Longitude)
        {
            double A = 0;
            double eccSquared = 0;
            double k0 = 0;
            double LongOrigin = 0;
            double eccPrimeSquared = 0;
            double N = 0;
            double t = 0;
            double C = 0;
            double M = 0;
            double LongTemp = 0;
            double LatRad = 0;
            double LongRad = 0;
            double LongOriginRad = 0;
            double pi = 0;
            double deg2rad = 0;
            double rad2deg = 0;
            double AA = 0;

            pi = 3.14159265;
            deg2rad = pi / 180.0;
            rad2deg = 180.0 / pi;
            AA = 6378137.0;
            eccSquared = 0.00669438;
            k0 = 0.9996;
            LongTemp = (Longitude + 180.0) - (int)Math.Truncate((Longitude + 180.0) / 360.0) * 360.0 - 180.0;
            LatRad = Latitude * deg2rad;
            LongRad = LongTemp * deg2rad;
            LongOrigin = (14.0 - 1.0) * 6.0 - 180.0 + 3.0;
            LongOriginRad = LongOrigin * deg2rad;
            eccPrimeSquared = (eccSquared) / (1.0 - eccSquared);
            N = AA / Math.Sqrt(1 - eccSquared * Math.Sin(LatRad) * Math.Sin(LatRad));
            t = Math.Tan(LatRad) * Math.Tan(LatRad);
            C = eccPrimeSquared * Math.Cos(LatRad) * Math.Cos(LatRad);
            A = Math.Cos(LatRad) * (LongRad - LongOriginRad);
            M = AA * ((1.0 - eccSquared / 4.0 - 3.0 * eccSquared * eccSquared / 64.0 - 5.0 * eccSquared * eccSquared * eccSquared / 256.0) * LatRad - (3.0 * eccSquared / 8.0 + 3.0 * eccSquared * eccSquared / 32.0 + 45.0 * eccSquared * eccSquared * eccSquared / 1024.0) * Math.Sin(2.0 * LatRad) + (15.0 * eccSquared * eccSquared / 256.0 + 45.0 * eccSquared * eccSquared * eccSquared / 1024.0) * Math.Sin(4.0 * LatRad) - (35.0 * eccSquared * eccSquared * eccSquared / 3072.0) * Math.Sin(6.0 * LatRad));
            double UTMEasting = (k0 * N * (A + (1.0 - t + C) * A * A * A / 6.0 + (5.0 - 18.0 * t + t * t + 72.0 * C - 58.0 * eccPrimeSquared) * A * A * A * A * A / 120.0) + 500000.0);
            double UTMNorthing = k0 * (M + N * Math.Tan(LatRad) * (A * A / 2.0 + (5.0 - t + 9.0 * C + 4.0 * C * C) * A * A * A * A / 24.0 + (61.0 - 58.0 * t + t * t + 600.0 * C - 330.0 * eccPrimeSquared) * A * A * A * A * A * A / 720.0));

            return new double[] { UTMEasting, UTMNorthing };
        }

    }
}