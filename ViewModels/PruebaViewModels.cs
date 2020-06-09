using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class PruebaViewModels
    {
        [Required]
        public int idPrueba { get; set; }
        [Required]
        public int? idUsuario { get; set; }
        [Required]
        public int? idTipoPregunta { get; set; }

        public bool? esSimulacion { get; set; }
        public bool? completado { get; set; }
    }
}