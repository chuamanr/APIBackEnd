using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class DatosEducacionViewModels
    {
        //public string institucion { get; set; }
        //public int idnivelEducativo { get; set; }
        //public string titulo { get; set; }
        //public string area_estudio { get; set; }
        //public string estado_estudio { get; set; }
        //public int idciudad_residencia { get; set; }
        //public DateTime fecha_inicio_estudio { get; set; }
        //public DateTime fecha_termino_estudio { get; set; }

        public int idInformacionEducativa { get; set; }
        public int? idDatosPersonales { get; set; }

        public int? idUsuario { get; set; }

        public int? dicAreaEstudio { get; set; }

        public int? dicCiudadEstudio { get; set; }
        public string institucionEducativa { get; set; }
        public int? dicNivelEducativo { get; set; }
        public string tituloObtenido { get; set; }
        public string estadoEstudio { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public DateTime? fechaActualizacion { get; set; }
        public int? dicTipoEstudio { get; set; }
        public bool? esComplementario { get; set; }
        public int? dicPaisExtranjero { get; set; }
        public string descripcionEducacion { get; set; }


    }
}
