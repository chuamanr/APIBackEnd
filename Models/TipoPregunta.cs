namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipoPregunta")]
    public partial class TipoPregunta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TipoPregunta()
        {
            Pregunta = new HashSet<Pregunta>();
            Prueba = new HashSet<Prueba>();
            TipoPreguntaInforme = new HashSet<TipoPreguntaInforme>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idTipoPregunta { get; set; }

        [StringLength(200)]
        public string nombreTipoPregunta { get; set; }

        public bool? vigente { get; set; }

        [StringLength(200)]
        public string descripcion { get; set; }

        [StringLength(50)]
        public string tipoEvaluacion { get; set; }

        public int? cantidadPreguntasSimulacion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pregunta> Pregunta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prueba> Prueba { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TipoPreguntaInforme> TipoPreguntaInforme { get; set; }
    }
}
