using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class AplicacionOfertasViewModel
    {
        public int idOferta { get; set; }
        public int idUsuario { get; set; }
        public bool vigente { get; set; }
    }
}