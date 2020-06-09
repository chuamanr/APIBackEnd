namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DiccionarioDatos
    {
        [Key]
        public int IdDiccionarioDatos { get; set; }

        [StringLength(200)]
        public string nombreDiccionario { get; set; }

        public int? tipoDiccionario { get; set; }

        public bool? habilitarOtro { get; set; }

        public int? estadoDiccionario { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public bool? vigente { get; set; }

        public int? departamento { get; set; }

        public virtual TipoDiccionario TipoDiccionario1 { get; set; }
    }
}
