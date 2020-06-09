namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LogRecuperacionContrasenaSMS
    {
        [Key]
        public int idLog { get; set; }

        [StringLength(50)]
        public string identificacion { get; set; }

        [StringLength(100)]
        public string celular { get; set; }

        [StringLength(160)]
        public string mensajeSMS { get; set; }

        [StringLength(150)]
        public string response { get; set; }
    }
}
