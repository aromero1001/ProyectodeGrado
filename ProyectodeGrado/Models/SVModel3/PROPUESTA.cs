namespace ProyectodeGrado.Models.SVModel3
{
    using SVModel3;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PROPUESTA")]
    public partial class PROPUESTA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROPUESTA()
        {
            OPCION_PROPUESTA = new HashSet<OPCION_PROPUESTA>();
        }

        [Key]
        public int ID_PROPUESTA { get; set; }

        public int? ID_TIPO_ELECCION { get; set; }

        [Required]
        [StringLength(50)]
        public string TITULO { get; set; }

        [StringLength(250)]
        public string DESCRIPCION { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPCION_PROPUESTA> OPCION_PROPUESTA { get; set; }

        public virtual TIPO_ELECCION TIPO_ELECCION { get; set; }
    }
}
