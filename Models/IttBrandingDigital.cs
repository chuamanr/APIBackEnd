namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IttBrandingDigital")]
    public partial class IttBrandingDigital
    {
        [Key]
        public int idBranding { get; set; }

        public int? idEmpresa { get; set; }

        public int? IdUsuario { get; set; }

        public int? idAgenda { get; set; }

        public int? estadoBranding { get; set; }

        [StringLength(300)]
        public string descripcionServicios { get; set; }

        public string identidadDigital { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public bool? vigente { get; set; }

        public virtual IttEmpresa IttEmpresa { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
