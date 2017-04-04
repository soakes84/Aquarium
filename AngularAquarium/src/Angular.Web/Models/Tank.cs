using Aquarium.Data;
using Aquarium.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Angular.Web.Models
{
    public class Tank
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public virtual List<Fish> Fishes { get; set; }

        public Tank()
        {
            var Fishes = new List<Fish>();
        }
    }
}
