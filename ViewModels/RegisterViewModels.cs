using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class RegisterViewModels
    {
        public string identificacion { get; set; }
        public int tipoIdentificacion { get; set; }
        public string tokenCaptcha { get; set; }
    }

    public class UpdateViewModels
    {
        public string fono { get; set; }
        public int idusuario { get; set; }
        public string email { get; set; }
        public string clave{ get; set; }
        public string tokenCaptcha { get; set; }
    }


    public class DataRespuestaViewModels
    {
        public string fono { get; set; }
        public int idusuario { get; set; }
        public string email { get; set; }
        public string identificacion { get; set; }
        public string paterno { get; set; }
        public string materno { get; set; }
        public string nombre { get; set; }
        public int idTipoIdentificacion { get; set; }
        public string nombreTipoIdentificacion { get; set; }
    }

    public class PreguntasSicotecnicasViewModels
    {
        public int? tipoPregunta { get; set; }
        public bool? completado { get; set; }
    }

    public class RequestInstafitViewModels
    {
        public string uid { get; set; }
        public string coupon { get; set; }
    }

    public class CouponInstafitViewModels
    {
        public string coupon_code { get; set; }
        public string source { get; set; }
        public string uid { get; set; }
        public string token { get; set; }
    }
}