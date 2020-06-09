using System;

namespace EcoOplacementApi.ViewModels
{
    public class OfertaViewModels
    {
        public int idOferta { get; set; }

        public int? idUsuarioModificador { get; set; }

        public int? idUsuario { get; set; }

        public string tituloOferta { get; set; }

        public string descripcionOferta { get; set; }

        public string salarioOferta { get; set; }

        public string ciudadOferta { get; set; }

        public DateTime? fechaPublicacionOferta { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public int? estadoOferta { get; set; }

        public bool? vigente { get; set; }

        public string link { get; set; }

        public string idOfertaProveedor { get; set; }

        public string proveedorEmpleo { get; set; }

        public string sector { get; set; }

        public string tipoContrato { get; set; }

        public string razonSocial { get; set; }
        public string cantidadVacantes { get; set; }

        public string requisitos { get; set; }

        public string experienciaRequerida { get; set; }

        public DateTime? fechaVencimiento { get; set; }
        public string area { get; set; }
    }
}