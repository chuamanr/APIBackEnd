namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IttIncripcionEvento")]
    public partial class IttIncripcionEvento
    {
        [Key]
        public int idInscripcionEvento { get; set; }

        public int? idEvento { get; set; }

        public int? IdUsuario { get; set; }

        public int? idEmpresa { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public bool? vigente { get; set; }

        public int? estadoIncripcion { get; set; }

        public virtual IttEventosNetworking IttEventosNetworking { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
