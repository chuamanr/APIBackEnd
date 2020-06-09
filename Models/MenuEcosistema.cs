namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MenuEcosistema")]
    public partial class MenuEcosistema
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuEcosistema()
        {
            MenuItemEcosistema = new HashSet<MenuItemEcosistema>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idMenuEcosistema { get; set; }

        [StringLength(250)]
        public string textoMenu { get; set; }

        [StringLength(500)]
        public string imagenRutaMenu { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public int? orden { get; set; }

        public bool? vigente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuItemEcosistema> MenuItemEcosistema { get; set; }
    }
}
