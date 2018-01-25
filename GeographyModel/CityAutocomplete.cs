using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GeographyModel
{
    [NotMapped]
    public class CityAutocomplete
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Display { get; set; }
    }
   
}
