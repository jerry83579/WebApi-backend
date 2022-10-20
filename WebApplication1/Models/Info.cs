using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Info
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Food { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double? Lat { get; set; }
        public double? Longitude { get; set; }
        public Geometry Location { get; set; }
    }
}