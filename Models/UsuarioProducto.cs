namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioProducto")]
    public partial class UsuarioProducto
    {
        [Key]
        public int IdUsuarioProducto { get; set; }

        [StringLength(50)]
        public string IdentificacionUsuario { get; set; }

        [StringLength(10)]
        public string IdProducto { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public bool? Vigente { get; set; }
    }
}
