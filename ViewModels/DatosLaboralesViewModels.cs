using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class DatosLaboralesViewModels
    {
        //public string Empresa{ get; set; }
        //public string Cargo { get; set; }
        //public string Funciones_Logros { get; set; }
        //public DateTime Fecha_Ingreso{ get; set; }
        //public DateTime? Fecha_Salida { get; set; }
        //public string Trabajo_Actualmente { get; set; }
        //public string Habilidades_Aptitudes { get; set; }
        //public string Descripcion_Perfil { get; set; }
        public int idExperienciaLaboral { get; set; }

        public int? idDatosPersonales { get; set; }
        public string empresa { get; set; }
        public string cargo { get; set; }
        public string funcionesLogros { get; set; }
        public string trabajoActualmente { get; set; }

        public DateTime? fechaIngreso { get; set; }
        public DateTime? fechaSalida { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public DateTime? fechaActualizacion { get; set; }
    }
}