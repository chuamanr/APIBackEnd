namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mensaje")]
    public partial class Mensaje
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idMensaje { get; set; }

        public string textoMensaje { get; set; }
    }
}
