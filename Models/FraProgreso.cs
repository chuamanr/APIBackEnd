namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FraProgreso")]
    public partial class FraProgreso
    {
        [Key]
        public int idProgreso { get; set; }

        public int idCurso { get; set; }

        public int? IdNivel { get; set; }

        public int? idUsuario { get; set; }

        public DateTime? fechaCreacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaInico { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaFin { get; set; }

        public bool? vigente { get; set; }

        public int? estadoNivel { get; set; }

        public virtual FraCurso FraCurso { get; set; }

        public virtual FraNiveles FraNiveles { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
