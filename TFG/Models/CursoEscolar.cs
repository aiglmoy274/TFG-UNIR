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
    /// Clase del modelo que representa los cursos escolares
    /// </summary>
    public class CursoEscolar
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Propiedad que contiene la descripción del curso escolar
        /// </summary>
        [Required(ErrorMessage ="El campo descripción es obligatorio.")]
        [StringLength(50, ErrorMessage = "No puede superar los 50 caracteres.")]        
        [Display(Name = "Curso Escolar")]
        public string Descripcion { get; set; }

        /// <summary>
        /// Propiedad que contiene el nivel educativo al que pertenece el curso escolar
        /// </summary>
        [Display(Name = "Nivel Educativo")]
        public int NivelEducativoId { get; set; }
        public NivelEducativo NivelEducativo { get; set; }

        /// <summary>
        /// Propiedad que contiene la colección de asignaturas que pertecen a la especialidad        
        /// </summary>
        public ICollection<Asignatura> Asignaturas { get; set; }
        public ICollection<Grupo> Grupos { get; set; }

    }
}
