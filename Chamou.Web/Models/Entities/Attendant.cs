using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chamou.Web.Models.Entities
{
    public class Attendant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Place Place { get; set; }

        public int PlaceId { get; set; }

    }
}