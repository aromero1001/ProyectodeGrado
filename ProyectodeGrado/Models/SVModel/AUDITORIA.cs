namespace ProyectodeGrado.Models.SVModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AUDITORIA")]
    public partial class AUDITORIA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO { get; set; }

        public bool? ESTATUS_VOTO { get; set; }

        public int? ID_VOTO { get; set; }

        public DateTime? FECHA_HORA { get; set; }

        public virtual USUARIO USUARIO { get; set; }
    }
}
