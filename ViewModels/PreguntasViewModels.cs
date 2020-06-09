using System.Collections.Generic;

namespace EcoOplacementApi.ViewModels
{
    public class PreguntasViewModels
    {
        public int idPregunta { get; set; }

        public int? idTipoPregunta { get; set; }

        public string glosaPregunta { get; set; }

        public bool? vigente { get; set; }

        public int? Orden { get; set; }

        public List<PreguntaRespuestaViewModels> lista { get; set; }
    }

    public class PreguntaRespuestaViewModels
    {
        public int idPreguntaRespuesta { get; set; }

        public int? idPregunta { get; set; }

        public string glosaPreguntaRespuesta { get; set; }

        public int? orden { get; set; }

        public bool? esCorrecta { get; set; }

    }
}