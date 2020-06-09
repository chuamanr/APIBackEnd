namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SaludFinancieraPreguntas
    {
        [Key]
        public int idPregunta { get; set; }

        public int? idTipoPregunta { get; set; }

        [StringLength(400)]
        public string glosaPregunta { get; set; }

        public bool? vigente { get; set; }

        public int? ordenPregunta { get; set; }

        [StringLength(50)]
        public string campoRespuesta { get; set; }

        public short? tabPagina { get; set; }
    }
}
