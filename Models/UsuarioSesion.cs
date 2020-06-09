namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UsuarioSesion")]
    public partial class UsuarioSesion
    {
        [Key]
        public int idUsuarioSesion { get; set; }

        public int? idUsuario { get; set; }

        public string token { get; set; }

        public DateTime? fechaSesion { get; set; }

        [StringLength(50)]
        public string ip { get; set; }

        public Nullable<Boolean> vigente  { get; set; }
    }
}
