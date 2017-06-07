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
    /// Clase del modelo que representa al Nivel Educativo
    /// </summary>
    public class NivelEducativo
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Propiedad que contiene la descripción del nivel educativo
        /// </summary>
        [Required(ErrorMessage = "El campo Descripción es obligatorio.")]
        [StringLength(50, ErrorMessage = "No puede superar los 50 caracteres.")]        
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que contiene la colección de Cursos escolares que pertecen al nivel educativo
        /// </summary>
        public ICollection<CursoEscolar> CursosEscolares { get; set; }

        /// <summary>
        /// Propiedad que contiene la colección de Planes de Estudio que pertecen al nivel educativo
        /// </summary>
        public ICollection<PlanEstudio> PlanesEstudio { get; set; }

    }
}
