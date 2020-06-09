namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contacto")]
    public partial class Contacto
    {
        public int id { get; set; }

        public int idUsuario { get; set; }

        [Required]
        [StringLength(30)]
        public string tipoContacto { get; set; }

        [Column("contacto")]
        [Required]
        [StringLength(80)]
        public string contacto1 { get; set; }

        public bool? vigente { get; set; }
    }
}
