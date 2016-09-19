using System;

namespace Dynamic.GeographicCalcService
{
    public class ErrorClass
    {
        private GeoCalcErrors _Error = GeoCalcErrors.Geo_to_Legal;
        public enum GeoCalcErrors
        {
            Geo_to_Legal = 0,
            Legal_to_Geo_using_Footage = 1,
            Legal_to_Geo_using_Subsection = 2,
            Legal_to_Geo_using_Section = 3,
            Outside_of_Section = 4,
            Unable_to_Calculate = -1,
            Outside_of_Nebraska = -2,
            Error_Reading_Database = -3,
            Invalid_Section_Corners = -4,
            Error_Outside_of_Section = -5,
            Not_Enough_Information = -6,
            Error_in_Footage_Calc = -10,
            Error_in_Subsection_Calc = -11,
            Error_in_Section_Calc = -12,
            Unknown_Error = -20,
        }

        public ErrorClass()
        {
            Code = 0;
        }

        private string ErrorCodes(GeoCalcErrors InCode)
        {
            //int test;
            switch (InCode)
            {
                case GeoCalcErrors.Geo_to_Legal:
                    return "Geographic to Legal.";
                case GeoCalcErrors.Legal_to_Geo_using_Footage:
                    return "Legal to Geographic using footage.";
                case GeoCalcErrors.Legal_to_Geo_using_Subsection:
                    return "Legal to Geographic using subsection.";
                case GeoCalcErrors.Legal_to_Geo_using_Section:
                    return "Legal to Geographic using section.";
                case GeoCalcErrors.Unable_to_Calculate:
                    return "Unable to calculate.";
                case GeoCalcErrors.Outside_of_Nebraska:
                    return "Coordinate appears to be outside the state of Nebraska.";
                case GeoCalcErrors.Error_Reading_Database:
                    return "Error reading database.";
                case GeoCalcErrors.Invalid_Section_Corners:
                    return "Invalid section corners.";
                case GeoCalcErrors.Error_in_Footage_Calc:
                    return "Error calculating the geographic coordinates using footage.";
                case GeoCalcErrors.Error_in_Subsection_Calc:
                    return "Error calculating the geographic coordinates using subsection.";
                case GeoCalcErrors.Error_in_Section_Calc:
                    return "Error calculating the geographic coordinates using section.";
                case GeoCalcErrors.Error_Outside_of_Section:
                    return "The calculated point end up outside of the section. Footage may be to large.";
                case GeoCalcErrors.Not_Enough_Information:
                    return "Not Enough Information to calculate position.";
                default:
                    return "An unknown error occurred.";
            }
        }

        public int Code
        {
            get
            {
                return Convert.ToInt32(_Error);
            }
            set
            {
                _Error = (GeoCalcErrors)value;
            }
        }

        public string Description
        {
            get
            {
                return ErrorCodes(_Error);
            }
        }

    }
}