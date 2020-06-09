using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class entRespuesta
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public string mensajeExtra { get; set; }
        public object data { get; set; }
    }
}