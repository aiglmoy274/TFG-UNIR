using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TFG.Models.ViewModels
{
    public class UsuarioViewModel
    {

        public string Id { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string CodigoCentro { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string NombreCentro { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string DescripcionComunidadAutonoma { get; set; }

        [Display(Name = "Comunidad Autónoma")]
        public int ComunidadAutonomaId { get; set; }

        [Display(Name = "Perfil")]
        public string PerfilNombre { get; set; }

    }
}
