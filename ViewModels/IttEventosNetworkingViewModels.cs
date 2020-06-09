using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class IttEventosNetworkingViewModels
    {
        public int idEvento { get; set; }

        public int? idCiudad { get; set; }

        public int? idTipoEvento { get; set; }


        public string descripcionEvento { get; set; }

        public string nombreConferencista { get; set; }

        public string descripcionConferencista { get; set; }

        public string fechaEvento { get; set; }

        public TimeSpan? HoraEvento { get; set; }

        public string lugarEvento { get; set; }

        public string nombreArchivo { get; set; }
        public string rutaImagenEvento { get; set; }
    }
}