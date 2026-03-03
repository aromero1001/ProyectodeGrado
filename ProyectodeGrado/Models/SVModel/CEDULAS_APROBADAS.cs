namespace ProyectodeGrado.Models.SVModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CEDULAS_APROBADAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CEDULAS_APROBADAS()
        {
            USUARIO = new HashSet<USUARIO>();
        }

        [Key]
        [StringLength(8)]
        public string CEDULA { get; set; }

        [Required]
        [StringLength(20)]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(20)]
        public string APELLIDO { get; set; }

        public int? ID_ROL { get; set; }

        public virtual ROLES ROLES { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO> USUARIO { get; set; }
    }
}
