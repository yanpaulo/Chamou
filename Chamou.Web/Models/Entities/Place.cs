using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace Chamou.Web.Models.Entities
{
    public class Place
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public virtual DbGeography Location { get; set; }
        
        public double CenterLatitude { get; set; }

        public double CenterLongitude { get; set; }

        public virtual ICollection<GeoPoint> LocationPoints { get; set; }

    }
}