namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PlantillaCorreo")]
    public partial class PlantillaCorreo
    {
        public int Id { get; set; }

        [Required]
        public string Plantilla { get; set; }
    }
}
