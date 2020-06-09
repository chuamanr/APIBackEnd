namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalarioSector")]
    public partial class SalarioSector
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idSector { get; set; }

        [StringLength(100)]
        public string glosaSector { get; set; }
    }
}
