//Autor: AGUSTÍN IGLESIAS MOYA
//Fecha: 22/04/2017

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TFG.Models
{
    /// <summary>
    /// Clase del modelo que representa las comunidades autónomas
    /// </summary>
    public class ComunidadAutonoma
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int ComunidadAutonomaId { get; set; }

        /// <summary>
        /// Propiedad que contiene la descripción de la comunidad autónoma
        /// </summary>
        [Required(ErrorMessage = "El campo Descripción es obligatorio")]
        [StringLength(50, ErrorMessage = "No puede superar los 50 caracteres.")]        
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que contiene la colección de usuarios que pertecen a la comunidad autónoma        
        /// </summary>
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        /// <summary>
        /// Propiedad que contiene la colección de planes de estudio que pertecen a la comunidad autónoma        
        /// </summary>
        public ICollection<PlanEstudio> PlanesEstudios { get; set; }
    }
}
