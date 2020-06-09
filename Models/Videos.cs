namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Videos
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string titulo { get; set; }

        [StringLength(1000)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(200)]
        public string url { get; set; }

        [StringLength(30)]
        public string nivel { get; set; }

        [StringLength(50)]
        public string tiempo { get; set; }

        public bool vigente { get; set; }
    }
}
