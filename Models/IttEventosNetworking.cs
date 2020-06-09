namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IttEventosNetworking")]
    public partial class IttEventosNetworking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IttEventosNetworking()
        {
            IttIncripcionEvento = new HashSet<IttIncripcionEvento>();
        }

        [Key]
        public int idEvento { get; set; }

        public int? idCiudad { get; set; }

        public int? idTipoEvento { get; set; }

        public byte[] rutaImagenEvento { get; set; }

        [StringLength(2000)]
        public string descripcionEvento { get; set; }

        [StringLength(200)]
        public string nombreConferencista { get; set; }

        [StringLength(1000)]
        public string descripcionConferencista { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaEvento { get; set; }

        public TimeSpan? HoraEvento { get; set; }

        [StringLength(500)]
        public string lugarEvento { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public bool? vigente { get; set; }

        public virtual CiudadCodigosDANE CiudadCodigosDANE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IttIncripcionEvento> IttIncripcionEvento { get; set; }
    }
}
