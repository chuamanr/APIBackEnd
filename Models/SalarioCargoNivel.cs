namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalarioCargoNivel")]
    public partial class SalarioCargoNivel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idCargoNivel { get; set; }

        [StringLength(100)]
        public string glosaCargoNivel { get; set; }
    }
}
