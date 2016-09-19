using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;

namespace Dynamic.GeographicCalcService
{
    public class DatabaseMod
    {
        /// <summary>
        /// Load the section corners from the database using the legal description.
        /// </summary>
        /// <param name="Location"></param>
        public static TRSClass LoadSectionFromDatabase(TRSClass Location)
        {
            //TRSClass output = Location.Clone();
                var sections = from s in GetAllSections()
                               where s.Township == Location.Township &&
                                     s.Range == Location.Range &&
                                     s.RangeDir == Location.RangeDirection.Direction &&
                                     s.Section == Location.Section
                               select s;
                if (sections.Count() != 1) return Location;
                var section = sections.Single();
                Location.Corners.SetPoint(0, section.UTMURX, section.UTMURY);
                Location.Corners.SetPoint(1, section.UTMULX, section.UTMULY);
                Location.Corners.SetPoint(2, section.UTMLLX, section.UTMLLY);
                Location.Corners.SetPoint(3, section.UTMLRX, section.UTMLRY);

            return Location;
        }

        /// <summary>
        /// Load the section corners from the database using the latitude and longitude.
        /// </summary>
        /// <param name="Location"></param>
        public static TRSClass LoadUsingLatLong(TRSClass Location)
        {
            // first part of search fuction uses sql to find all possible sections
            // Can return 0 to 4 possible sections
                var sections = from s in GetAllSections()
                               where (s.UTMURX > Location.Point.X || s.UTMLRX > Location.Point.X) &&
                                     (s.UTMULX < Location.Point.X || s.UTMLLX < Location.Point.X) &&
                                     (s.UTMULY > Location.Point.Y || s.UTMURY > Location.Point.Y) &&
                                     (s.UTMLLY < Location.Point.Y || s.UTMLRY < Location.Point.Y)
                               select s;

                foreach (var section in sections)
                {

                    Location.Township = section.Township;
                    Location.Range = section.Range;
                    Location.RangeDirection.Direction = section.RangeDir;
                    Location.Section = section.Section;
                    Location.Corners.SetPoint(0, section.UTMURX, section.UTMURY);
                    Location.Corners.SetPoint(1, section.UTMULX, section.UTMULY);
                    Location.Corners.SetPoint(2, section.UTMLLX, section.UTMLLY);
                    Location.Corners.SetPoint(3, section.UTMLRX, section.UTMLRY);

                    if (Location.Corners.IsWithIn(Location.Point))
                        break;
                }

            return Location;
        }

        private static List<SectionCorners> GetAllSections()
        {
            List<SectionCorners> sections = new List<SectionCorners>();

            //using (ZipArchive archive = ZipFile.OpenRead("SectionCorners.zip"))
            //{
            //    foreach (ZipArchiveEntry entry in archive.Entries)
            //    {
            //        if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            //        {
            //            entry.ExtractToFile(Path.Combine(extractPath, entry.FullName));
            //        }
            //    }
            //}




            return sections;
        }
    }
}
