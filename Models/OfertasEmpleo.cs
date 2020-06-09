namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OfertasEmpleo")]
    public partial class OfertasEmpleo
    {
        [Key]
        public int idOferta { get; set; }

        public int? idUsuarioModificador { get; set; }

        public int? idUsuario { get; set; }

        [StringLength(500)]
        public string tituloOferta { get; set; }

        [StringLength(5000)]
        public string descripcionOferta { get; set; }

        [StringLength(500)]
        public string salarioOferta { get; set; }

        [StringLength(3000)]
        public string ciudadOferta { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaPublicacionOferta { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public int? estadoOferta { get; set; }

        public bool? vigente { get; set; }

        [StringLength(500)]
        public string link { get; set; }

        [StringLength(50)]
        public string idOfertaProveedor { get; set; }

        [StringLength(50)]
        public string proveedorEmpleo { get; set; }

        [StringLength(5000)]
        public string sector { get; set; }

        [StringLength(50)]
        public string tipoContrato { get; set; }

        [StringLength(150)]
        public string razonSocial { get; set; }

        [StringLength(20)]
        public string cantidadVacantes { get; set; }

        [StringLength(5000)]
        public string requisitos { get; set; }

        [StringLength(50)]
        public string experienciaRequerida { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaVencimiento { get; set; }

        [StringLength(500)]
        public string area { get; set; }
        public int idOfertaPlacement { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
