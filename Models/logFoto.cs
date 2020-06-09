namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("logFoto")]
    public partial class logFoto
    {
        [Key]
        public int idlog { get; set; }

        public string texto { get; set; }
    }
}
