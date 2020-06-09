namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EjercicioAhorro")]
    public partial class EjercicioAhorro
    {
        public int id { get; set; }

        public int? idUsuario { get; set; }

        [StringLength(50)]
        public string articulo { get; set; }

        public double? valor { get; set; }

        [StringLength(50)]
        public string plazo { get; set; }

        public double? tasa_interes { get; set; }

        public double? ahorro_mensual { get; set; }

        public double? ahorro_anual { get; set; }

        public int? edad { get; set; }

        public double? ahorro_inversion { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
