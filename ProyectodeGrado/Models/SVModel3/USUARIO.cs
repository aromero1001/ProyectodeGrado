namespace ProyectodeGrado.Models.SVModel3
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USUARIO")]
    public partial class USUARIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USUARIO()
        {
            CANDIDATO = new HashSet<CANDIDATO>();
            HISTORIAL_LOGIN = new HashSet<HISTORIAL_LOGIN>();
            PREGUNTAS_SEGURIDAD = new HashSet<PREGUNTAS_SEGURIDAD>();
        }

        [Key]

        public int ID_USUARIO { get; set; }


        public int ID_ESTATUS { get; set; }

        [Required]
        [StringLength(8)]
        [Display(Name = "Cedula: ")]
        public string CEDULA { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Usuario:")]
        public string NOMBRE_USUARIO { get; set; }

        [Required]
        [StringLength(16)]
        [Display(Name = "Contraseña:  ")]
        public string CLAVE { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "Email:  ")]
        public string CORREO { get; set; }

        [Display(Name = "Foto:  ")]
        public byte[] FOTO_PERFIL { get; set; }

        [StringLength(100)]
        [Display(Name = "Descripcion:  ")]
        public string DESCRIPCION { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Nombre: ")]
        public string NOMBRE { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Apellido:  ")]
        public string APELLIDO { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tipo:")]
        public string TIPO_ESTUDIO { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = " Nivel:")]
        public string NIVEL_ESTUDIO { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Facultad/Seccion:")]
        public string FACULTAD_SECCION { get; set; }

        [Required]

        [StringLength(1)]
        [Display(Name = "Genero")]
        public string GENERO { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Fecha:")]
        public DateTime FECHA_NACIMIENTO { get; set; }



        public int ID_ROL { get; set; }



        public int? INTENTOS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CANDIDATO> CANDIDATO { get; set; }

        public virtual CEDULAS_APROBADAS CEDULAS_APROBADAS { get; set; }

        public virtual ESTATUS_USUARIO ESTATUS_USUARIO { get; set; }

        public virtual ROLES ROLES { get; set; }

        public virtual AUDITORIA AUDITORIA { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HISTORIAL_LOGIN> HISTORIAL_LOGIN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PREGUNTAS_SEGURIDAD> PREGUNTAS_SEGURIDAD { get; set; }
    }
}