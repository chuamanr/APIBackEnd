namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FraNiveles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FraNiveles()
        {
            FraProgreso = new HashSet<FraProgreso>();
        }

        [Key]
        public int idNivel { get; set; }

        public int? idCurso { get; set; }

        [StringLength(1000)]
        public string contenidoTexto { get; set; }

        [StringLength(1000)]
        public string urlVideo { get; set; }

        public int? ordenNivel { get; set; }

        public DateTime? fechaCrecion { get; set; }

        public bool? vigente { get; set; }

        public virtual FraCurso FraCurso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FraProgreso> FraProgreso { get; set; }
    }
}
