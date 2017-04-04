using Angular.Web.Models;
using Aquarium.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Aquarium.Models
{
    public class Fish
    {
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public Tank Tank { get; set; }
        public int TankId { get; set; }
        public ApplicationUser Owner { get; set; }
    }
}
