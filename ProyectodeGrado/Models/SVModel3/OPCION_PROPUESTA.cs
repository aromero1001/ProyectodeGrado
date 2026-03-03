namespace ProyectodeGrado.Models.SVModel3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OPCION_PROPUESTA
    {
        [Key]
        public int ID_OPCION { get; set; }

        public int ID_PROPUESTA { get; set; }

        [Required]
        [StringLength(30)]
        public string OPCION { get; set; }

        public virtual PROPUESTA PROPUESTA { get; set; }
    }
}
