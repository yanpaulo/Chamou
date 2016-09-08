using System;
using System.Collections.Generic;
using System.Linq;

namespace Chamou.WindowsApp.Models
{
    public class Place
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public double CenterLatitude { get; set; }

        public double CenterLongitude { get; set; }

        public virtual ICollection<GeoPoint> LocationPoints { get; set; }

    }
}