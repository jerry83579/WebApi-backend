using NetTopologySuite.Geometries;

namespace WebApplication1.Models
{
    public class requestBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Food { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double? Lat { get; set; }
        public double? Longitude { get; set; }

    }
}
