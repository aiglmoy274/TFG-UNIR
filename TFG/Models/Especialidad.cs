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
    /// Clase del modelo que representa las especialidades del profesorado
    /// </summary>
    public class Especialidad
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Propiedad que contiene el código de la especialidad
        /// </summary>
        [Required(ErrorMessage ="El campo Código de especialidad es obligatorio")]
        [StringLength(6, ErrorMessage = "No puede superar los 6 caracteres.")]
        [Display(Name = "Código especialidad")]
        public string Numero { get; set; }

        /// <summary>
        /// Propiedad que contiene la descripción de la especialidad
        /// </summary>
        [Required(ErrorMessage = "El campo Descripción es obligatorio")]
        [StringLength(50, ErrorMessage = "No puede superar los 50 caracteres.")]        
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que contiene la colección de asignaturas que pertecen a la especialidad
        /// </summary>
        public ICollection<Asignatura> Asignaturas { get; set; }

        public ICollection<PlanificacionReduccion> PlanificacionesReduccion { get; set; }
        public ICollection<PlanificacionAsignatura> PlanificacionesAsignatura { get; set; }

    }
}
