using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class HtmlExperianViewModels
    {
        public class Calificacione
        {
            public string calificacion { get; set; }
            public string texto { get; set; }
            public string calificacionLetra { get; set; }
            public string textoMejoramiento { get; set; }
        }

        public class Endeudamiento
        {
            public double porcentajeUtilizacion { get; set; }
            public double porcentajePendiente { get; set; }
            public string comportamiento { get; set; }
            public double prestamosPlazoFijo { get; set; }
            public List<Calificacione> calificaciones { get; set; }
        }

        public class Calificacione2
        {
            public string calificacion { get; set; }
            public string texto { get; set; }
            public string calificacionLetra { get; set; }
            public string textoMejoramiento { get; set; }
        }

        public class EstadoPortafolio
        {
            public int numeroProductosAbiertosAlDia { get; set; }
            public bool tieneOTuvoCreditosHipotecarios { get; set; }
            public bool tieneProductosFinancierosOTeleco { get; set; }
            public int cuentasComoCodeudor { get; set; }
            public List<Calificacione2> calificaciones { get; set; }
        }

        public class Calificacione3
        {
            public string calificacion { get; set; }
            public string texto { get; set; }
            public string calificacionLetra { get; set; }
            public string textoMejoramiento { get; set; }
        }

        public class ExperienciaCrediticia
        {
            public int aniosExperienciaCrediticia { get; set; }
            public int mesesDesdeNuevoProducto { get; set; }
            public int numeroObligacionesUltimos6Meses { get; set; }
            public int totalProductosCerrados { get; set; }
            public List<Calificacione3> calificaciones { get; set; }
        }

        public class Calificacione4
        {
            public string calificacion { get; set; }
            public string texto { get; set; }
            public string calificacionLetra { get; set; }
            public string textoMejoramiento { get; set; }
        }

        public class HabitoPago
        {
            public int productosConMora30DiasUltimos12Meses { get; set; }
            public int productosConMora60DiasUltimos48Meses { get; set; }
            public double saldoEnMora { get; set; }
            public int productosEnMoraALaFecha { get; set; }
            public List<Calificacione4> calificaciones { get; set; }
        }

        public class DiagnosticInfo
        {
            public Endeudamiento endeudamiento { get; set; }
            public EstadoPortafolio estadoPortafolio { get; set; }
            public ExperienciaCrediticia experienciaCrediticia { get; set; }
            public HabitoPago habitoPago { get; set; }
        }

        public class Razon
        {
            public string codigo { get; set; }
        }

        public class Score
        {
            public string tipo { get; set; }
            public string puntaje { get; set; }
            public DateTime fecha { get; set; }
            public string poblacion { get; set; }
            public List<Razon> Razon { get; set; }
        }

        public class RootObject
        {
            public string scoreHTML { get; set; }
            public string diagnoseHTML { get; set; }
            public DiagnosticInfo diagnosticInfo { get; set; }
            public Score score { get; set; }
        }
    }
}