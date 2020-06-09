using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class AgendamientoViewModels
    {
        public int idAgenda { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? idCasoMoksys { get; set; }

        public int? idAgendaTipo { get; set; }
        public string DiscriminadorAgendamiento { get; set; }
        public int? idUsuario { get; set; }
        public string contacto { get; set; }
        public DateTime? fechaActualizacion { get; set; }

        public DateTime? fechaAgenda { get; set; }

        public TimeSpan? horaAgenda { get; set; }

    }
}