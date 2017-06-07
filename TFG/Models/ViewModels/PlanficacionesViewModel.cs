using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFG.Models.ViewModels
{
    public class PlanficacionesViewModel
    {
        public Planificacion Planificacion { get; set; }
        public int PlanificacionId { get; set; }        

        public PlanificacionReduccion PlanificacionReduccion { get; set; }

        public ICollection<PlanificacionReduccion> PlanificacionesReduccion { get; set; }
        
        public Grupo Grupo { get; set; }
        public ICollection<Grupo> Grupos { get; set; }

        public PlanificacionAsignatura PlanificacionAsignatura { get; set; }

        public ICollection<ResultadosViewModel> Resultados { get; set; }

    }
}
