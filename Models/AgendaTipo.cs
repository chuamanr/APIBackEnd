namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AgendaTipo")]
    public partial class AgendaTipo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AgendaTipo()
        {
            Agenda = new HashSet<Agenda>();
            Coworking = new HashSet<Coworking>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idAgendaTipo { get; set; }

        [StringLength(100)]
        public string glosaAgendaTipo { get; set; }

        public bool? vigente { get; set; }

        [StringLength(100)]
        public string modulo { get; set; }

        public int? idTipoServicioMoksys { get; set; }

        public int? idRequerimientoMoksys { get; set; }

        public int? idServicioMoksys { get; set; }

        public int? idContratoMoksys { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agenda> Agenda { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Coworking> Coworking { get; set; }
    }
}
