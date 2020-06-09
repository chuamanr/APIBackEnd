namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MenuItemEcosistema")]
    public partial class MenuItemEcosistema
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idMenuItemEcosistema { get; set; }

        public int? idMenuEcosistema { get; set; }

        [StringLength(200)]
        public string textoMenuItem { get; set; }

        [StringLength(200)]
        public string urlMenuItem { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public int? orden { get; set; }

        public int? idUsuario { get; set; }

        public bool? vigente { get; set; }

        public virtual MenuEcosistema MenuEcosistema { get; set; }
    }
}
