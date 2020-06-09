namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FraCurso")]
    public partial class FraCurso
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FraCurso()
        {
            FraNiveles = new HashSet<FraNiveles>();
            FraProgreso = new HashSet<FraProgreso>();
        }

        [Key]
        public int idCurso { get; set; }

        [StringLength(5000)]
        public string descripcionCurso { get; set; }

        [StringLength(100)]
        public string imagenCurso { get; set; }

        [StringLength(1000)]
        public string descripcionCorta { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public bool? vigente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FraNiveles> FraNiveles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FraProgreso> FraProgreso { get; set; }
    }
}
