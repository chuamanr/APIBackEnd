namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Coworking")]
    public partial class Coworking
    {
        public int Id { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? dicCiudad { get; set; }

        public int? idAgendaTipo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaAgendamiento { get; set; }

        [StringLength(50)]
        public string horario { get; set; }

        public int? idUsuario { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public int? idCasoMoksys { get; set; }

        public bool? vigente { get; set; }

        public virtual AgendaTipo AgendaTipo { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
