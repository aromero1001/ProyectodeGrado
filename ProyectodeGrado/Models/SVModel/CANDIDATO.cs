namespace ProyectodeGrado.Models.SVModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CANDIDATO")]
    public partial class CANDIDATO
    {
        [Key]
        public int ID_MIEMBRO { get; set; }

        public int ID_USUARIO { get; set; }

        public int? ID_PARTIDO { get; set; }

        public virtual PARTIDO PARTIDO { get; set; }

        public virtual USUARIO USUARIO { get; set; }
    }
}
