using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.Filters.Security
{
    public class RequestLoginJWT
    {
        public string usuario { get; set; }
        public string password { get; set; } 
        public string sitioUsuario { get; set; }
        public string sitioPassword { get; set; }
        public string ip { get; set; }
        public string tokenCaptcha { get; set; }
    }
}