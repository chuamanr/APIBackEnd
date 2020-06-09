namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SaludFinancieraTipo")]
    public partial class SaludFinancieraTipo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idSaludFinancieraTipo { get; set; }

        [StringLength(100)]
        public string nombreSaludFinancieraTipo { get; set; }

        [StringLength(300)]
        public string descripcion { get; set; }

        public bool? vigente { get; set; }

        public double? ponderacion { get; set; }

        public int? sectionEcosistema { get; set; }

        public int? puntaje { get; set; }
    }
}
