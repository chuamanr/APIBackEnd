using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class PruebaPreguntaViewModels
    {
        public int idPruebaRespuesta { get; set; }

        public int? idPrueba { get; set; }

        public int? idPregunta { get; set; }

        public int? idPreguntaRespuesta { get; set; }

        public DateTime? fechaPruebaRespuesta { get; set; }

        public bool? vigente { get; set; }

        public string respuestaLibre { get; set; }
    }
}