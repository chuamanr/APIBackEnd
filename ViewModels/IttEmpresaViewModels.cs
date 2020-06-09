using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class IttEmpresaViewModels
    {
        public int idEmpresa { get; set; }

        public int? idUsuario { get; set; }

        public int? idSector { get; set; }

        public string nombreEmpresa { get; set; }

        public string descripcionEmpresa { get; set; }

        public string mailEmpresarial { get; set; }
        public string telefonoEmpresarial { get; set; }

        //public string idLogo { get; set; }

        //public byte[] logoEmpresa { get; set; }

        public string paginaWeb { get; set; }

        public DateTime? fechaHoraCreacion { get; set; }
        public string rutaImagen { get; set; }
        public bool? vigente { get; set; }
    }
}