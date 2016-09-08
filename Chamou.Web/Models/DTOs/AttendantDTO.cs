using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chamou.Web.Models.DTOs
{
    public class AttendantDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string PlaceName { get; set; }

        public int PlaceId { get; set; }
    }
}