namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalarioCargo")]
    public partial class SalarioCargo
    {
        [Key]
        [StringLength(20)]
        public string idCargo { get; set; }

        [StringLength(100)]
        public string glosaCargo { get; set; }
    }
}
