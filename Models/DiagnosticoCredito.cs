namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DiagnosticoCredito")]
    public partial class DiagnosticoCredito
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Documento { get; set; }

        [Required]
        public string RespustaDiagnostico { get; set; }

        [Required]
        public string LogConsumo { get; set; }

        public DateTime FechaConsumo { get; set; }
    }
}
