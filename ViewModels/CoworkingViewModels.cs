using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class CoworkingViewModels
    {
        public int Id { get; set; }

        public int? dicCiudad { get; set; }
        public int? idAgendaTipo { get; set; }
        public DateTime? fechaAgendamiento { get; set; }
        public string horario { get; set; }

        public int? idUsuario { get; set; }

        public int? idCasoMoksys { get; set; }
    }
}