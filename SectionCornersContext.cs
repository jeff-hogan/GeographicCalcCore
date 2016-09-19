using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dynamic.GeographicCalcService
{
    [Table("SectionCorners")]
    public class SectionCorners
    {
        [Key]
        public long ID { get; set; }
        public int Township { get; set; }
        public int Range { get; set; }
        public string RangeDir { get; set; }
        public int Section { get; set; }
        public double UTMURX { get; set; }
        public double UTMURY { get; set; }
        public double UTMULX { get; set; }
        public double UTMULY { get; set; }
        public double UTMLLX { get; set; }
        public double UTMLLY { get; set; }
        public double UTMLRX { get; set; }
        public double UTMLRY { get; set; }
    }
}