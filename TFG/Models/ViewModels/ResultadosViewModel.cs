using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TFG.Models.ViewModels
{
    public class ResultadosViewModel
    {
        public Especialidad Especialidad { get; set; }
        public int EspecialidadId { get; set; }      
        public int HorasSemanalesReduccion { get; set; }
        public int HorasSemanalesAsignaturas { get; set; }
        public int TotalHorasSemanales { get; set; }
        public int NumProfesores { get; set; }
        public int HorasFaltanCuadrarProfesor { get; set; }

    }
}
