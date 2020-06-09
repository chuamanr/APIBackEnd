namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Agenda = new HashSet<Agenda>();
            AplicacionesOfertas = new HashSet<AplicacionesOfertas>();
            Coworking = new HashSet<Coworking>();
            EjercicioAhorro = new HashSet<EjercicioAhorro>();
            FraProgreso = new HashSet<FraProgreso>();
            HojaVidaDatosPersonales = new HashSet<HojaVidaDatosPersonales>();
            HojaVidaInformacionEducativa = new HashSet<HojaVidaInformacionEducativa>();
            IttBrandingDigital = new HashSet<IttBrandingDigital>();
            IttEmpresa = new HashSet<IttEmpresa>();
            IttIncripcionEvento = new HashSet<IttIncripcionEvento>();
            LogActividad = new HashSet<LogActividad>();
            OfertasEmpleo = new HashSet<OfertasEmpleo>();
            Prueba = new HashSet<Prueba>();
        }

        [Key]
        public int idUsuario { get; set; }

        public int? dicTipoDocumento { get; set; }

        [StringLength(200)]
        public string identificacion { get; set; }

        [StringLength(50)]
        public string nombres { get; set; }

        [StringLength(50)]
        public string apellidoPaterno { get; set; }

        [StringLength(50)]
        public string apellidoMaterno { get; set; }

        [StringLength(100)]
        public string correoElectronico { get; set; }

        [StringLength(50)]
        public string numeroCelular { get; set; }

        public string clave { get; set; }

        public string responseJSON { get; set; }

        public bool? validacionData { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaCreacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaActualizacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fechaCambioClave { get; set; }

        public int? estadoUsuario { get; set; }

        public int? tipoEstado { get; set; }

        public DateTime? fechaUltimoLogin { get; set; }

        public int? cantidadIntentos { get; set; }

        public DateTime? ultimaFechaCambioClave { get; set; }

        public int? diasRestantesCambioClave { get; set; }

        public int? codigo { get; set; }

        public DateTime? fechaExpiracionCodigo { get; set; }

        public bool? usuarioBloqueado { get; set; }

        public DateTime? fechaAccesoExperian { get; set; }

        public string responseDataExperian { get; set; }

        [StringLength(150)]
        public string idCoursera { get; set; }

        public string dataCoursera { get; set; }

        public bool? habilitarNotificacion { get; set; }

        [StringLength(50)]
        public string telefonoNotificacion { get; set; }

        [StringLength(50)]
        public string correoNotificacion { get; set; }

        public bool? vigente { get; set; }

        public string instafitUrl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agenda> Agenda { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AplicacionesOfertas> AplicacionesOfertas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Coworking> Coworking { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EjercicioAhorro> EjercicioAhorro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FraProgreso> FraProgreso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaDatosPersonales> HojaVidaDatosPersonales { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HojaVidaInformacionEducativa> HojaVidaInformacionEducativa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IttBrandingDigital> IttBrandingDigital { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IttEmpresa> IttEmpresa { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IttIncripcionEvento> IttIncripcionEvento { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogActividad> LogActividad { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfertasEmpleo> OfertasEmpleo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prueba> Prueba { get; set; }
    }
}
