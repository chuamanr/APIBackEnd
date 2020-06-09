using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class DataPreguntasViewModels
    {
        public int IdPregunta { get; set; }
        public string glosaPregunta { get; set; }
        public int? numeroPagina { get; set; }
        public DataPreguntasRespuestasViewModels[] Data { get; set; }
    }

    public class DataPreguntasRespuestasViewModels
    {
        public int idPreguntaRespuesta { get; set; }
        public int idPregunta { get; set; }
        public string glosaPreguntaRespuesta { get; set; }
        public int orden { get; set; }
        public bool esCorrecta { get; set; }
    }
}