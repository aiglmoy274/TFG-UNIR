using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TFG.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string CodigoCentro { get; set; }
        public string NombreCentro { get; set; }

        public int ComunidadAutonomaId { get; set; }
        public ComunidadAutonoma ComunidadAutonoma { get; set; }

        public ICollection<Planificacion> Planificaciones { get; set; }

    }
}
