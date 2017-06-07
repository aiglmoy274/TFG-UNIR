using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TFG.Models
{ 

      
    public class Asignatura
    {
        public int Id { get; set; }


        [Required(ErrorMessage ="El campo descripción es obligatorio")]
        [StringLength(50, ErrorMessage = "No puede superar los 50 caracteres.")]        
        [Display(Name = "Descripión")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo horas semanales es obligatorio")]
        [Range(1,30,ErrorMessage = "Las horas semanales deben estar comprendidas entre 1 y 30")]
        [Display(Name = "Horas semanales")]
        public int HorasSemanales { get; set; }

        [Display(Name = "Plan de Estudio")]
        public int PlanEstudioId { get; set; }
        public PlanEstudio PlanEstudio { get; set; }

        [Display(Name = "Curso Escolar")]
        public int CursoEscolarId { get; set; }
        public CursoEscolar CursoEscolar { get; set; }

        [Display(Name = "Especialidad")]
        public int EspecialidadId { get; set; }
        public Especialidad Especialidad { get; set; }

        public ICollection<PlanificacionAsignatura> PlanificacionesAsignatura { get; set; }


    }
}
