namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AplicacionesOfertas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idOferta { get; set; }

        public int? idUsuario { get; set; }

        public DateTime? fechaAplicacion { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public bool? vigente { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
