namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogActividad")]
    public partial class LogActividad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idLogActividad { get; set; }

        public int? dicTipoLog { get; set; }

        public int? idUsuario { get; set; }

        [StringLength(20)]
        public string ipAcceso { get; set; }

        public string consultaJSON { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaCreacionRegistro { get; set; }

        public bool? vigente { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
