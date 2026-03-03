namespace ProyectodeGrado.Models.SVModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HISTORIAL_LOGIN
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SESION { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime FECHA_INICIO { get; set; }

        public DateTime? FECHA_FIN { get; set; }

        public virtual USUARIO USUARIO { get; set; }
    }
}
