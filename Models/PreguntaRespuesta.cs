namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PreguntaRespuesta")]
    public partial class PreguntaRespuesta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PreguntaRespuesta()
        {
            PruebaRespuesta = new HashSet<PruebaRespuesta>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idPreguntaRespuesta { get; set; }

        public int? idPregunta { get; set; }

        [StringLength(250)]
        public string glosaPreguntaRespuesta { get; set; }

        public int? orden { get; set; }

        public bool? esCorrecta { get; set; }

        public int? peso { get; set; }

        public virtual Pregunta Pregunta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PruebaRespuesta> PruebaRespuesta { get; set; }
    }
}
