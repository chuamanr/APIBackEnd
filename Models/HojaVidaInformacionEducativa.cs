namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HojaVidaInformacionEducativa")]
    public partial class HojaVidaInformacionEducativa
    {
        [Key]
        public int idInformacionEducativa { get; set; }

        public int? idDatosPersonales { get; set; }

        public int? idUsuario { get; set; }

        public string dicAreaEstudio { get; set; }

        public string dicCiudadEstudio { get; set; }

        public string dicIntensidadEstudio { get; set; }

        public string dicNivelEducativo { get; set; }

        public string dicTipoEstudio { get; set; }

        public string dicPaisExtranjero { get; set; }

        public string institucionEducativa { get; set; }

        public string tituloObtenido { get; set; }
        
        public string estadoEstudio { get; set; }
        
        public string otroEstudio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaInicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaFin { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public bool? vigente { get; set; }

        public bool? esComplementario { get; set; }

        public string descripcionEducacion { get; set; }

        public virtual HojaVidaDatosPersonales HojaVidaDatosPersonales { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
