using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TFG.Models;

namespace TFG.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<TFG.Models.ComunidadAutonoma> ComunidadAutonoma { get; set; }
        public DbSet<TFG.Models.Especialidad> Especialidad { get; set; }
        public DbSet<TFG.Models.NivelEducativo> NivelEducativo { get; set; }
        public DbSet<TFG.Models.CursoEscolar> CursoEscolar { get; set; }
        public DbSet<TFG.Models.PlanEstudio> PlanEstudio { get; set; }
        public DbSet<TFG.Models.Asignatura> Asignatura { get; set; }
        public DbSet<TFG.Models.Planificacion> Planificacion { get; set; }
        public DbSet<TFG.Models.PlanificacionReduccion> PlanificacionReduccion { get; set; }
        public DbSet<TFG.Models.PlanificacionAsignatura> PlanificacionAsignatura { get; set; }
        public DbSet<TFG.Models.Grupo> Grupo { get; set; }




    }
}
