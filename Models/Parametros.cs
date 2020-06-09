namespace EcoOplacementApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Parametros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idParametro { get; set; }

        [StringLength(100)]
        public string tipoParametro { get; set; }

        public string parametroJSON { get; set; }

        public static implicit operator string(Parametros v)
        {
            throw new NotImplementedException();
        }
    }
}
