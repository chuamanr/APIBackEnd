using EcoOplacementApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class RespuestaViewModels
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public DataRespuestaViewModels user { get; set; }
    }

    public class CambiarClaveViewModels
    {
        public string idUsuario { get; set; }
        public string clave { get; set; }
        public string claveAnterior { get; set; }
    }
    public class UsuarioViewModels
    {
        public string idUsuario { get; set; }

        public int? dicTipoDocumento { get; set; }

        public string identificacion { get; set; }

        public string nombres { get; set; }

        public string apellidoPaterno { get; set; }

        public string apellidoMaterno { get; set; }


        public string correoElectronico { get; set; }

        public string numeroCelular { get; set; }

 
        public string clave { get; set; }

        public bool? validacionData { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public DateTime? fechaCambioClave { get; set; }

        public int? estadoUsuario { get; set; }

        public int? tipoEstado { get; set; }

        public DateTime? fechaUltimoLogin { get; set; }

        public int? cantidadIntentos { get; set; }

        public DateTime? ultimaFechaCambioClave { get; set; }

        public int? diasRestantesCambioClave { get; set; }

        public bool? vigente { get; set; }

        public string instafitUrl { get; set; }

        public int? Codigo { get; set; }

        public DateTime? FechaExpiracionCodigo { get; set; }

        public List<string> perfiles { get; set; }
        public List<string> socios { get; set; }
    }





    public class entHojaVida
    {
        public int idDatosPersonales { get; set; }
        public int idUsuario { get; set; }
        public int IdPlantillaAplicada { get; set; }
        public int dicCiudadResidencia { get; set; }
        public int dicDepartamento { get; set; }
        public int dicTipoDocumento { get; set; }
        public int dicProfesion { get; set; }
        public int dicAreaTrabajo { get; set; }
        public int dicAspiracionSalarial { get; set; }
        public string nombres { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string rutaFoto { get; set; }
        public string identificacion { get; set; }
        public string correoElectronico { get; set; }
        public string telefonoCelular { get; set; }
        public string direccionResidencia { get; set; }
        public string anosExperiencia { get; set; }
        public string paisesTrabajar { get; set; }
        public string redesSociales { get; set; }
        public string descripcionPerfilProfesional { get; set; }
        public string genero { get; set; }
        public string movilidadLaboral { get; set; }
        public string idiomaNivel { get; set; }
        public string habilidades { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaActualizacion { get; set; }
        public string tipoTextoHV { get; set; }
        public string colorHV { get; set; }
        public bool vigente { get; set; }
        public List<entExperienciaLaboral> EntExperienciaLaboral { get; set; }
        public List<entHojaVidaInformacionEducativa> EntHojaVidaInformacionEducativa { get; set; }

        public entHojaVida()
        {
            EntExperienciaLaboral = new List<entExperienciaLaboral>();
            EntHojaVidaInformacionEducativa = new List<entHojaVidaInformacionEducativa>();
        }
    }

    public class entExperienciaLaboral
    {
        public int idExperienciaLaboral { get; set; }
        public int idDatosPersonales { get; set; }
        public string empresa { get; set; }
        public string cargo { get; set; }
        public string funcioneslogros { get; set; }
        public string trabajoActualmente { get; set; }
        public DateTime fechaIngreso { get; set; }
        public DateTime fechaSalida { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public bool vigente { get; set; }
    }

    public class entHojaVidaInformacionEducativa
    {
        public int idInformacionEducativa { get; set; }
        public int idDatosPersonales { get; set; }
        public int idUsuario { get; set; }
        public int dicAreaEstudio { get; set; }
        public int dicCiudadEstudio { get; set; }
        public int dicIntensidadEstudio { get; set; }
        public string institucionEducativa { get; set; }
        public int? dicNivelEducativo { get; set; }
        public string tituloObtenido { get; set; }
        public string estadoEstudio { get; set; }
        public string otroEstudio { get; set; }
        public DateTime fechaInicio { get; set; }
        public DateTime fechaFin { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaActualizacion { get; set; }
        public int? dicTipoEstudio { get; set; }
        public bool vigente { get; set; }

        public bool? esComplementario { get; set; }
        public int? dicPaisExtranjero { get; set; }
        public string descripcionEducacion { get; set; }
    }

    public class CodigoRespuestaViewModels
    {
        public int id { get; set; }
        public int code { get; set; }
        public string tokenCaptcha { get; set; }
    }

    public class ConfirmaViewModels
    {
        public int id { get; set; }
        public string password { get; set; }
        public int code { get; set; }
        public string tokenCaptcha { get; set; }
    }

    public class RecuperarContraseña
    {
        public string identificacion { get; set; }
        public string tokenCaptcha { get; set; }
    }

    public class ArchivoViewModels
    {
        public int idUsuario { get; set; }
        public string imagenBase64 { get; set; }
        public string tipoArchivo { get; set; }
        public string idImagen { get; set; }
    }

    public class RespuestaAgendaViewModels
    {
        public int idUser { get; set; }
        public int idAgenda { get; set; }
       
    }

    public class ContactoViewModels
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string tipoContacto { get; set; }
        public string contacto { get; set; }
    }

    public class ExperianViewModels
    {
        public string password { get; set; }
        public string username { get; set; }
        public string grant_type { get; set; }
        public string documentType { get; set; }
        public string document { get; set; }
        public string lastName { get; set; }
    }
    public class DataSentinelViewModels
    {
        public string Gx_UsuEnc { get; set; }
        public string Gx_PasEnc { get; set; }
        public string Gx_Key { get; set; }
        public string TipoDoc { get; set; }
        public string NroDoc { get; set; }
    }
    public class AlertaExperianViewModels
    {
        public string password { get; set; }
        public string username { get; set; }
        public string grant_type { get; set; }
        public string identificacion { get; set; }
    }

}
