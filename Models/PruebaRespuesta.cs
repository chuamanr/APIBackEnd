namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PruebaRespuesta")]
    public partial class PruebaRespuesta
    {
        [Key]
        public int idPruebaRespuesta { get; set; }

        public int? idPrueba { get; set; }

        public int? idPregunta { get; set; }

        public int? idPreguntaRespuesta { get; set; }

        public DateTime? fechaPruebaRespuesta { get; set; }

        public bool? vigente { get; set; }

        [StringLength(50)]
        public string respuestaLibre { get; set; }

        public virtual Pregunta Pregunta { get; set; }

        public virtual PreguntaRespuesta PreguntaRespuesta { get; set; }

        public virtual Prueba Prueba { get; set; }
    }
}
