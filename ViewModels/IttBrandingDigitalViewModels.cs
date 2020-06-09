using System;

namespace EcoOplacementApi.ViewModels
{
    public class IttBrandingDigitalViewModels
    {
        public int idBranding { get; set; }

        public int? idEmpresa { get; set; }

        public int? IdUsuario { get; set; }

        public int? idAgenda { get; set; }

        public int? estadoBranding { get; set; }
        public string descripcionServicios { get; set; }

        public string identidadDigital { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public bool? vigente { get; set; }

        
    }
}