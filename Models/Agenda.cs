namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Agenda
    {
        [Key]
        public int idAgenda { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? idCasoMoksys { get; set; }

        [StringLength(50)]
        public string discriminadorAgendamiento { get; set; }

        public int? idUsuario { get; set; }

        [StringLength(50)]
        public string contacto { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public DateTime? fechaAgenda { get; set; }

        public TimeSpan? horaAgenda { get; set; }

        public bool? vigente { get; set; }

        public int? idAgendaTipo { get; set; }

        public virtual AgendaTipo AgendaTipo { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
