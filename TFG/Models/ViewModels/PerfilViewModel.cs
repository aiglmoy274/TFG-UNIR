using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TFG.Models.ViewModels
{
    public class PerfilViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Perfil")]
        public string PerfilNombre { get; set; }
        
    }
}
