﻿using Chamou.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chamou.Web.Models.DTOs
{
    public class PlaceDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public double CenterLatitude { get; set; }

        public double CenterLongitude { get; set; }

        public virtual ICollection<GeoPoint> LocationPoints { get; set; }

        public virtual ICollection<AttendantDTO> Attendants { get; set; }
    }
}