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
    /// Clase del modelo que representa al Plan de Estudio
    /// </summary>
    public class PlanEstudio
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// propiedad que contiene la descripción del plan de estudio
        /// </summary>
        [Required(ErrorMessage ="El campo Descripción es obligatorio")] //Campo obligatorio
        [StringLength(50, ErrorMessage = "No puede superar los 50 caracteres.")] //Logitud máxima de 50 caracteres        
        [Display(Name = "Descripión")]//Dato a mostrar como etiqueta en los formularios
        public string Descripcion { get; set; }
        
        /// <summary>
        /// Propiedad que contiene la normativa en la que se basa el plan de estudio
        /// </summary>
        [StringLength(200, ErrorMessage = "No puede superar los 200 caracteres.")] //Longitud máxima de 200 caracteres
        [Display(Name = "Normativa")]//Dato a mostrar como etiqueta en los formularios
        public string Normativa { get; set; }

        /// <summary>
        /// Propiedad que contiene la comunidad autónoma a la que pertence el plan de estudio
        /// </summary>
        [Display(Name = "Comunidad Autónoma")]
        public int ComunidadAutonomaId { get; set; }
        public ComunidadAutonoma ComunidadAutonoma { get; set; }
        
        /// <summary>
        /// Propiedad que contiene el nivel educativo al que pertence el plan de estudio
        /// </summary>
        [Display(Name = "Nivel Educativo")]
        public int NivelEducativoId { get; set; }
        public NivelEducativo NivelEducativo { get; set; }
        
        /// <summary>
        /// Propiedad que contiene la colección de Asignaturas que pertecen a este plan de estudio
        /// </summary>
        public ICollection<Asignatura> Asignaturas { get; set; }

    }
}
