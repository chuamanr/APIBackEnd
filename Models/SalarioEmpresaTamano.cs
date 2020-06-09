namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalarioEmpresaTamano")]
    public partial class SalarioEmpresaTamano
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idEmpresaTamano { get; set; }

        [StringLength(100)]
        public string glosaEmpresaTamano { get; set; }
    }
}
