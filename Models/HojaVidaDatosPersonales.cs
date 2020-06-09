namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HojaVidaDatosPersonales
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HojaVidaDatosPersonales()
        {
            HojaVidaExperienciaLaboral = new HashSet<HojaVidaExperienciaLaboral>();
            HojaVidaInformacionEducativa = new HashSet<HojaVidaInformacionEducativa>();
        }

        [Key]
        public int idDatosPersonales { get; set; }

        public int? idUsuario { get; set; }

        public int? idPlantillaAplicada { get; set; }

        public int? dicCiudadResidencia { get; set; }

        public int? dicTipoDocumento { get; set; }

        public string dicProfesion { get; set; }

        public string dicAreaTrabajo { get; set; }

        public int? dicAspiracionSalarial { get; set; }

        [StringLength(50)]
        public string nombres { get; set; }

        [StringLength(50)]
        public string apellidoPaterno { get; set; }

        [StringLength(50)]
        public string apellidoMaterno { get; set; }

        [StringLength(50)]
        public string rutaFoto { get; set; }

        [StringLength(50)]
        public string identificacion { get; set; }

        [StringLength(50)]
        public string correoElectronico { get; set; }

        [StringLength(50)]
        public string telefonoCelular { get; set; }

        [StringLength(150)]
        public string direccionResidencia { get; set; }

        [StringLength(50)]
        public string anosExperiencia { get; set; }

        [StringLength(1000)]
        public string paisesTrabajar { get; set; }

        [StringLength(1000)]
        public string redesSociales { get; set; }

        [StringLength(1000)]
        public string descripcionPerfilProfesional { get; set; }

        public byte[] fotoPerfil { get; set; }

        [StringLength(100)]
        public string idImagen { get; set; }

        public byte[] pdfHojaDeVida { get; set; }

        [StringLength(1)]
        public string genero { get; set; }

        [StringLength(2)]
        public string movilidadLaboral { get; set; }

        [StringLength(1000)]
        public string idiomaNivel { get; set; }

        [StringLength(1000)]
        public string habilidades { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaNacimiento { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public bool? vigente { get; set; }

        public int? estadoHojaVida { get; set; }

        [StringLength(30)]
        public string tipoTextoHV { get; set; }

        [StringLength(30)]
        public string colorHV { get; set; }

        public int? dicDepartamento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaExperienciaLaboral> HojaVidaExperienciaLaboral { get; set; }

        public virtual PlantillaHojaDeVida PlantillaHojaDeVida { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaInformacionEducativa> HojaVidaInformacionEducativa { get; set; }
    }
}
