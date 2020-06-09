namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HojaVidaExperienciaLaboral")]
    public partial class HojaVidaExperienciaLaboral
    {
        [Key]
        public int idExperienciaLaboral { get; set; }

        public int? idDatosPersonales { get; set; }

        public string empresa { get; set; }

        public string cargo { get; set; }

        public string funcionesLogros { get; set; }

        public string trabajoActualmente { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaIngreso { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaSalida { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public bool? vigente { get; set; }

        public virtual HojaVidaDatosPersonales HojaVidaDatosPersonales { get; set; }
    }
}
