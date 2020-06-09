using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class DataBrandingViewModels
    {
        public int idBranding { get; set; }

        public int? idEmpresa { get; set; }

        public int? IdUsuario { get; set; }

        public int? idAgenda { get; set; }

        public int? estadoBranding { get; set; }
        public string descripcionServicios { get; set; }

        public string identidadDigital { get; set; }

        public int? idSector { get; set; }

        public string nombreEmpresa { get; set; }

        public string descripcionEmpresa { get; set; }

        public string mailEmpresarial { get; set; }
        public string telefonoEmpresarial { get; set; }

        public string paginaWeb { get; set; }

        public string idLogo { get; set; }

        public string logoEmpresa { get; set; }
        public string nombreArchivo { get; set; }
        public string rutaImagen { get; set; }
        public string nombreSector { get; set; }
    }
}