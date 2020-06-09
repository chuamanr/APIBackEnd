using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class CursosViewModels
    {
        public int idCurso { get; set; }
        public string descripcionCurso { get; set; }
        public string imagenCurso { get; set; }
        public string descripcionCorta { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public bool? vigente { get; set; }
    }
}