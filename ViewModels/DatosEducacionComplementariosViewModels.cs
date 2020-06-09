using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class DatosEducacionComplementariosViewModels
    {
        public int idInformacionEducativa { get; set; }

        public int? idDatosPersonales { get; set; }

        public int? idUsuario { get; set; }

        public int? dicAreaEstudio { get; set; }

        public int? dicCiudadEstudio { get; set; }

        public int? dicIntensidadEstudio { get; set; }
        public string institucionEducativa { get; set; }
        public string tituloObtenido { get; set; }
        public string estadoEstudio { get; set; }
        public string otroEstudio { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public DateTime? fechaActualizacion { get; set; }

        public int? dicTipoEstudio { get; set; }
        public bool? esComplementario { get; set; }
        public int? dicPaisExtranjero { get; set; }
        public string descripcionEducacion { get; set; }

    }
}
