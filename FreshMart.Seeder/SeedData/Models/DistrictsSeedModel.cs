using FreshMart.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshMart.Seeder.SeedData.Models
{
    public class DistrictsSeedModel : District
    {
        public string BnName { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public string Url { get; set; }
    }
}
