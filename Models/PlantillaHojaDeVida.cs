namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlantillaHojaDeVida")]
    public partial class PlantillaHojaDeVida
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlantillaHojaDeVida()
        {
            HojaVidaDatosPersonales = new HashSet<HojaVidaDatosPersonales>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idPlantilla { get; set; }

        [StringLength(200)]
        public string nombrePlantilla { get; set; }

        public string htmlPlantilla { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public int? estadoPlantilla { get; set; }

        [StringLength(500)]
        public string tagEstudiosPrincipales { get; set; }

        [StringLength(500)]
        public string tagEstudiosComplementarios { get; set; }

        [StringLength(500)]
        public string tagExperienciaLaboral { get; set; }

        [StringLength(500)]
        public string tagIdioma { get; set; }

        [StringLength(500)]
        public string tagRedes { get; set; }

        [StringLength(500)]
        public string tagMovilidadLaboral { get; set; }

        [StringLength(500)]
        public string tagFuncionesLogros { get; set; }

        public bool? vigente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaDatosPersonales> HojaVidaDatosPersonales { get; set; }
    }
}
