namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoPreguntaInforme")]
    public partial class TipoPreguntaInforme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idTipoPreguntaInforme { get; set; }

        public int? idTipoPregunta { get; set; }

        public int? puntajeDesde { get; set; }

        public int? puntajeHasta { get; set; }

        public string textoInforme { get; set; }

        public virtual TipoPregunta TipoPregunta { get; set; }
    }
}
