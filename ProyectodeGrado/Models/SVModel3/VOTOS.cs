namespace ProyectodeGrado.Models.SVModel3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VOTOS
    {
        [Key]
        public int ID_VOTO { get; set; }

        public bool ESTADO { get; set; }

        public int ID_ELECCION { get; set; }

        public int ID_SELECCIONADO { get; set; }

        public int? ID_PROPUESTA { get; set; }

        public DateTime FECHA_HORA { get; set; }

        [Required]
        [StringLength(1)]
        public string GENERO { get; set; }

        [Required]
        [StringLength(100)]
        public string FACULTAD { get; set; }

        [Required]
        [StringLength(30)]
        public string NIVEL_ESTUDIO { get; set; }

        public int EDAD { get; set; }

        public virtual ITEMS_ELECCION ITEMS_ELECCION { get; set; }

        public virtual TIPO_ELECCION TIPO_ELECCION { get; set; }
    }
}
