namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IttEmpresa")]
    public partial class IttEmpresa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IttEmpresa()
        {
            IttBrandingDigital = new HashSet<IttBrandingDigital>();
        }

        [Key]
        public int idEmpresa { get; set; }

        public int? idUsuario { get; set; }

        public int? idSector { get; set; }

        [StringLength(100)]
        public string nombreSector { get; set; }

        [StringLength(1000)]
        public string nombreEmpresa { get; set; }

        [StringLength(5000)]
        public string descripcionEmpresa { get; set; }

        [StringLength(100)]
        public string mailEmpresarial { get; set; }

        [StringLength(50)]
        public string telefonoEmpresarial { get; set; }

        public DateTime? fechaCreacion { get; set; }

        [StringLength(500)]
        public string paginaWeb { get; set; }

        [StringLength(300)]
        public string rutaImagen { get; set; }

        public bool? vigente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IttBrandingDigital> IttBrandingDigital { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
