namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ContingenciaPrimaria")]
    public partial class ContingenciaPrimaria
    {
        [Key]
        public int idContingenciaPrimaria { get; set; }

        [StringLength(40)]
        public string tipoDocumento { get; set; }

        [StringLength(20)]
        public string numeroIdentificacion { get; set; }

        public string numeroTelefono { get; set; }

        public string numeroTelefono2 { get; set; }

        public string numeroCelular { get; set; }

        [StringLength(200)]
        public string correoElectronico { get; set; }

        public string nombreUsuario { get; set; }

        [StringLength(100)]
        public string producto { get; set; }

        [StringLength(100)]
        public string cobertura { get; set; }

        [StringLength(100)]
        public string cobertura2 { get; set; }

        [StringLength(100)]
        public string cobertura3 { get; set; }

        [StringLength(12)]
        public string fechaSolicitud { get; set; }

        [StringLength(3)]
        public string codigoSubSocio { get; set; }

        [StringLength(50)]
        public string nombreSubsocio { get; set; }
    }
}
