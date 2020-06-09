namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CiudadCodigosDANE")]
    public partial class CiudadCodigosDANE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CiudadCodigosDANE()
        {
            IttEventosNetworking = new HashSet<IttEventosNetworking>();
        }

        [Key]
        public int IdCodigoDANE { get; set; }

        public int? codigoNivel1 { get; set; }

        [StringLength(10)]
        public string codigoNivel2 { get; set; }

        [StringLength(10)]
        public string codigoNivel3 { get; set; }

        [StringLength(60)]
        public string nombreNivel1 { get; set; }

        [StringLength(50)]
        public string nombreNivel2 { get; set; }

        [StringLength(50)]
        public string nombreNivel3 { get; set; }

        [StringLength(4)]
        public string tipo { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public bool? vigente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IttEventosNetworking> IttEventosNetworking { get; set; }
    }
}
