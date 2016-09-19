
namespace Dynamic.GeographicCalcService
{
    public class GeoCalcServiceFunctions
    {
        /// <summary>
        /// Calculate the Legal Location based off of the Geographic Coordinates.
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="ErrorObj"></param>
        public static TRSClass Geo2Legal(TRSClass Location)
        {
            //Load the section corners from the database.
            Location = DatabaseMod.LoadUsingLatLong(Location);
            //Check that the section corners are valid
            if (Location.Corners.IsValid())
            {
                //Location is valid calculate subsection and footage.
                Location.SubSection = Location.Corners.Point2SubSec(Location.Point);
                Location.Footage = Location.Corners.Point2Footage(Location.Point);
            }
            else
            {
                //Invalid section corners means that the location given is most likely 
                //outside of the state. So set error code and exit.
                Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Outside_of_Nebraska;
            }
            return Location;
        }

        /// <summary>
        /// Calculate the Geographic Coordinates based off of the best information in the legal description
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="ErrorObj"></param>
        public static TRSClass Legal2Geo(TRSClass Location)
        {
            //Try solving using the best information available in this order
            //  Footage
            //  Subsection
            //  Section

            //Check if the location is valid. If not set error code and exit.
            if (!Location.IsValid)
            {
                Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Not_Enough_Information;
                return Location;
            }
            //Load the section corners from the database.
            Location = DatabaseMod.LoadSectionFromDatabase(Location);

            // Check if the section corners are valid if not set error and exit.
            if (!Location.Corners.IsValid())
            {
                Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Invalid_Section_Corners;
                return Location;
            }

            //If footage is valid use it to calculate location.
            if (Location.Footage.IsValid())
            {
                Location.Point = Location.Corners.LocateFootage(Location.Footage);
                if ((Location.Point.X != -1))
                {
                    Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Legal_to_Geo_using_Footage;
                }
                else
                {
                    Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Error_in_Footage_Calc;
                }
            }
            //If subsection is valid use it to calculate the location.
            else if (Location.SubSection.IsValid())
            {
                Location.Point = Location.Corners.LocateSubSection(Location.SubSection);
                if ((Location.Point.X != -1))
                {
                    Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Legal_to_Geo_using_Subsection;
                }
                else
                {
                    Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Error_in_Subsection_Calc;
                }
            }
            //If the Section is valid use it to calculate the location.
            else if (Location.Corners.IsValid())
            {
                Location.Point = Location.Corners.Center();
                if ((Location.Point.X != -1))
                {
                    Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Legal_to_Geo_using_Section;
                }
                else
                {
                    Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Error_in_Section_Calc;
                }
            }
            else
            {
                Location.Point.X = -1;
                Location.Point.Y = -1;
                Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Unknown_Error;
            }
            if (Location.ErrorObj.Code > 0)
            {
                if (!Location.Corners.IsWithIn(Location.Point))
                {
                    //If the point is not within the section but is within 2 miles from the 
                    //center of the section consider it acceptable else throw an error.
                    if (!Location.Corners.IsNearSection(Location.Point))
                    {
                        Location.ErrorObj.Code = (int)GeographicCalcService.ErrorClass.GeoCalcErrors.Error_Outside_of_Section;
                    }
                }
            }
            return Location;
        }

    }
}