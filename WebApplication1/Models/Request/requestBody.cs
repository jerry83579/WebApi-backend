using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class requestBody
    {
        public int Id { get; set; }
        [JsonPropertyName("Name")]

        public string Name { get; set; }
        public string Food { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double Lat { get; set; }
        public double Longitude { get; set; }

    }
}
