using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace WebApplication1.Models
{
    public partial class ComplexQuery
    {
        public string Category { get; set; }
        public string Price { get; set; }
        public string Address { get; set; }
    }
}