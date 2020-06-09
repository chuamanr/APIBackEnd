namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        public int idErrorLog { get; set; }

        [StringLength(200)]
        public string evento { get; set; }

        [StringLength(500)]
        public string parametros { get; set; }

        [StringLength(2000)]
        public string errorMensaje { get; set; }
    }
}
