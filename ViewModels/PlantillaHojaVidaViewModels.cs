using System;

namespace EcoOplacementApi.ViewModels
{
    public class PlantillaHojaVidaViewModels
    {
        public int idPlantilla { get; set; }

   
        public string nombrePlantilla { get; set; }

        
        public string htmlPlantilla { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaActualizacion { get; set; }

        public int? estadoPlantilla { get; set; }


        public string tagEstudiosPrincipales { get; set; }

       
        public string tagEstudiosComplementarios { get; set; }

       
        public string tagExperienciaLaboral { get; set; }

        
        public string tagIdioma { get; set; }

       
        public string tagRedes { get; set; }

       
        public string tagMovilidadLaboral { get; set; }

      
        public string tagFuncionesLogros { get; set; }

        public bool? vigente { get; set; }
    }
}