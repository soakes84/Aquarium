using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Aquarium.Models;

namespace Aquarium.Data
{
    public class ApplicationUser : IdentityUser 
    {
        public List<Fish> Fishes { get; set; }

        public Guid Signature { get; set; }

    }
}
