using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TFG.Models
{ 

      
    public class Grupo
    {
        public int Id { get; set; }


        [Required(ErrorMessage ="El campo descripción es obligatorio")]
        [StringLength(50, ErrorMessage = "No puede superar los 50 caracteres.")]        
        [Display(Name = "Descripión")]
        public string Descripcion { get; set; }

        [Display(Name = "Curso Escolar")]
        public int CursoEscolarId { get; set; }
        public CursoEscolar CursoEscolar { get; set; }

        [Display(Name = "Planificacion")]
        public int PlanificacionId { get; set; }
        public Planificacion Planificacion { get; set; }

        public ICollection<PlanificacionAsignatura> PlanificacionesAsignatura { get; set; }


    }
}
