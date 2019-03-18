using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateXamarin.Models
{
    public class PropertyUpload
    {
        private string problem;

        public string Name { get; set; }
        public double Longitud { get; set; }
        public double Latitud { get; set; }
        public int Price { get; set; }
        public int Size { get; set; }
        public string Address { get; set; }
        public int Rooms { get; set; }
        public int ParkingSlots { get; set; }
        public int Bathrooms { get; set; }
        public string Description { get; set; }
        public List<FileData> Images { get; set; }

        public bool isValid() {
       

            if (string.IsNullOrEmpty(Name)) {           
                problem="Please set a Title for your property.";
                return false;
            }
            if (Longitud==0 || Latitud==0)
            {
                problem = "Please set a location.";
                return false;
            }
            if (Price == 0) {
                problem = "Please set a Price for your property.";
                return false;
            }
            if (Size == 0) {
                problem = "Please set the sqm size of your property.";
                return false;
            }
            if (String.IsNullOrEmpty(Address)) {
                problem = "Please set a location.";
                return false;
            }
            if (String.IsNullOrEmpty(Description)) {
                problem = "Please set a short description of your property.";
                return false;
            }
            if (Images != null)
            {
                if (Images.Count <= 0)
                {
                    problem = "Please set some images for the property.";
                    return false;
                }
            }
            else {
                problem = "Please set some images for the property.";
                return false;
            }

            return true;
        }
        public string GetProblem() {
            return problem;
        }
      
    }
}
