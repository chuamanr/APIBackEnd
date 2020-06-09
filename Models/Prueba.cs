namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prueba")]
    public partial class Prueba
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prueba()
        {
            PruebaRespuesta = new HashSet<PruebaRespuesta>();
        }

        [Key]
        public int idPrueba { get; set; }

        public int? idUsuario { get; set; }

        public int? idTipoPregunta { get; set; }

        public DateTime? fechaInicioPrueba { get; set; }

        public bool? esSimulacion { get; set; }

        public bool? vigente { get; set; }

        public bool? completado { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PruebaRespuesta> PruebaRespuesta { get; set; }

        public virtual TipoPregunta TipoPregunta { get; set; }
    }
}
