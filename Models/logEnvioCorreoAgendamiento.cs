namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("logEnvioCorreoAgendamiento")]
    public partial class logEnvioCorreoAgendamiento
    {
        [Key]
        public int idLog { get; set; }

        public DateTime? fechaLog { get; set; }

        public int? idUsuario { get; set; }

        public string plantilla { get; set; }

        public int? esTipo { get; set; }

        [StringLength(50)]
        public string fecha { get; set; }

        [StringLength(50)]
        public string hora { get; set; }

        public int? idAgendaTipo { get; set; }

        [StringLength(200)]
        public string contacto { get; set; }
    }
}
