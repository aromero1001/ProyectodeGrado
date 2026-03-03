namespace ProyectodeGrado.Models.SVModel3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PARTIDO")]
    public partial class PARTIDO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PARTIDO()
        {
            CANDIDATO = new HashSet<CANDIDATO>();
        }

        [Key]
        public int ID_PARTIDO { get; set; }

        public int? ID_TIPO_ELECCION { get; set; }

        [Required]
        [StringLength(50)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(250)]
        public string DESCRIPCION { get; set; }

        public byte[] FOTO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CANDIDATO> CANDIDATO { get; set; }

        public virtual TIPO_ELECCION TIPO_ELECCION { get; set; }
    }
}
