namespace ProyectodeGrado.Models.SVModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PREGUNTAS_SEGURIDAD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_USUARIO { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int N_PREGUNTA { get; set; }

        //[Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string PREGUNTA { get; set; }

        //[Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string RESPUESTA { get; set; }

        public virtual USUARIO USUARIO { get; set; }
    }
}
