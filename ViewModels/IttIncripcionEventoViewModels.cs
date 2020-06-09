using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class IttIncripcionEventoViewModels
    {
        public int idInscripcionEvento { get; set; }

        public int? idEvento { get; set; }

        public int? IdUsuario { get; set; }

        public int? idEmpresa { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public bool? vigente { get; set; }

        public int? estadoIncripcion { get; set; }
    }
}