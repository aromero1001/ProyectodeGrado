namespace ProyectodeGrado.Models.SVModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ELECCION")]
    public partial class ELECCION
    {
        [Key]
        public int ID_ELECCION { get; set; }

        [Required]
        [StringLength(60)]
        public string TITULO { get; set; }

        public DateTime FECHA_INICIO { get; set; }

        public DateTime FECHA_FIN { get; set; }

        [Required]
        [StringLength(50)]
        public string TIPO_INSTITUTO { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE_INSTITUTO { get; set; }

        [StringLength(250)]
        public string DESCRIPCION { get; set; }

        public byte[] LOGO { get; set; }

        public int DURACION_VOTO { get; set; }

        public int? ID_TIPO_ELECCION { get; set; }

        public virtual TIPO_ELECCION TIPO_ELECCION { get; set; }
    }
}
