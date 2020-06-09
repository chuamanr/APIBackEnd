namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoDiccionario")]
    public partial class TipoDiccionario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoDiccionario()
        {
            DiccionarioDatos = new HashSet<DiccionarioDatos>();
        }

        [Key]
        public int idTipoDiccionario { get; set; }

        [StringLength(50)]
        public string nombreTipoDiccionario { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public bool? vigente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DiccionarioDatos> DiccionarioDatos { get; set; }
    }
}
