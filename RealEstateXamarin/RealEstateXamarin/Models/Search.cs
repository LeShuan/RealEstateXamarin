using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateXamarin.Models
{
    public class Search
    {
        public int PriceMin { get; set; }
        public int PriceMax { get; set; }
        public int Distance { get; set; }
        public double Longitud { get; set; }
        public double Latitud { get; set; }
    }
}
