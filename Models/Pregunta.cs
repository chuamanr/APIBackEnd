namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pregunta")]
    public partial class Pregunta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pregunta()
        {
            PreguntaRespuesta = new HashSet<PreguntaRespuesta>();
            PruebaRespuesta = new HashSet<PruebaRespuesta>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idPregunta { get; set; }

        public int? idTipoPregunta { get; set; }

        [StringLength(250)]
        public string glosaPregunta { get; set; }

        public bool? vigente { get; set; }

        public int? Orden { get; set; }

        [StringLength(50)]
        public string respuestaLibre { get; set; }

        public short? numeroPagina { get; set; }

        public virtual TipoPregunta TipoPregunta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreguntaRespuesta> PreguntaRespuesta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PruebaRespuesta> PruebaRespuesta { get; set; }
    }
}
