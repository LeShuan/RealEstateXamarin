using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateXamarin.Models
{
    public class PropertyDetails
    {
        public int id { get; set; }

        public string Name { get; set; }

        public string MainImageURL { get; set; }

        public double Longitud { get; set; }

        public double Latitud { get; set; }

        public int Price { get; set; }

        public int Size { get; set; }

        public string Address { get; set; }

        public int Rooms { get; set; }

        public int ParkingSlots { get; set; }

        public int Bathrooms { get; set; }

        public string Description { get; set; }

        public List<Images> Images { get; set; }
    }
}
