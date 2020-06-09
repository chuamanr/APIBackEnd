namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalarioCiudad")]
    public partial class SalarioCiudad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idCiudad { get; set; }

        [StringLength(100)]
        public string glosaCiudad { get; set; }
    }
}