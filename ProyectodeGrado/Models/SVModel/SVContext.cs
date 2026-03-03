namespace ProyectodeGrado.Models.SVModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SVContext : DbContext
    {
        public SVContext()
            : base("name=SVContext")
        {
        }

        public virtual DbSet<CANDIDATO> CANDIDATO { get; set; }
        public virtual DbSet<ESTATUS_USUARIO> ESTATUS_USUARIO { get; set; }
        public virtual DbSet<ITEMS_ELECCION> ITEMS_ELECCION { get; set; }
        public virtual DbSet<OPCION_PROPUESTA> OPCION_PROPUESTA { get; set; }
        public virtual DbSet<PARTIDO> PARTIDO { get; set; }
        public virtual DbSet<PROPUESTA> PROPUESTA { get; set; }
        public virtual DbSet<ROLES> ROLES { get; set; }
        public virtual DbSet<TIPO_ELECCION> TIPO_ELECCION { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<VOTOS> VOTOS { get; set; }
        public virtual DbSet<AUDITORIA> AUDITORIA { get; set; }
        public virtual DbSet<CEDULAS_APROBADAS> CEDULAS_APROBADAS { get; set; }
        public virtual DbSet<ELECCION> ELECCION { get; set; }
        public virtual DbSet<HISTORIAL_LOGIN> HISTORIAL_LOGIN { get; set; }
        public virtual DbSet<PREGUNTAS_SEGURIDAD> PREGUNTAS_SEGURIDAD { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ESTATUS_USUARIO>()
                .Property(e => e.NOMBRE_ESTATUS)
                .IsUnicode(false);

            modelBuilder.Entity<ESTATUS_USUARIO>()
                .HasMany(e => e.USUARIO)
                .WithRequired(e => e.ESTATUS_USUARIO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ITEMS_ELECCION>()
                .HasMany(e => e.VOTOS)
                .WithRequired(e => e.ITEMS_ELECCION)
                .HasForeignKey(e => e.ID_SELECCIONADO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OPCION_PROPUESTA>()
                .Property(e => e.OPCION)
                .IsUnicode(false);

            modelBuilder.Entity<PARTIDO>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<PARTIDO>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<PROPUESTA>()
                .Property(e => e.TITULO)
                .IsUnicode(false);

            modelBuilder.Entity<PROPUESTA>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<PROPUESTA>()
                .HasMany(e => e.OPCION_PROPUESTA)
                .WithRequired(e => e.PROPUESTA)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ROLES>()
                .Property(e => e.NOMBRE_ROL)
                .IsUnicode(false);

            modelBuilder.Entity<ROLES>()
                .HasMany(e => e.USUARIO)
                .WithRequired(e => e.ROLES)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TIPO_ELECCION>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<TIPO_ELECCION>()
                .HasMany(e => e.ITEMS_ELECCION)
                .WithRequired(e => e.TIPO_ELECCION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TIPO_ELECCION>()
                .HasMany(e => e.VOTOS)
                .WithRequired(e => e.TIPO_ELECCION)
                .HasForeignKey(e => e.ID_ELECCION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.CEDULA)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.NOMBRE_USUARIO)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.CLAVE)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.CORREO)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.APELLIDO)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.TIPO_ESTUDIO)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.NIVEL_ESTUDIO)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.FACULTAD_SECCION)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .Property(e => e.GENERO)
                .IsUnicode(false);

            modelBuilder.Entity<USUARIO>()
                .HasMany(e => e.CANDIDATO)
                .WithRequired(e => e.USUARIO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USUARIO>()
                .HasOptional(e => e.AUDITORIA)
                .WithRequired(e => e.USUARIO);

            modelBuilder.Entity<USUARIO>()
                .HasMany(e => e.HISTORIAL_LOGIN)
                .WithRequired(e => e.USUARIO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USUARIO>()
                .HasMany(e => e.PREGUNTAS_SEGURIDAD)
                .WithRequired(e => e.USUARIO)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VOTOS>()
                .Property(e => e.GENERO)
                .IsUnicode(false);

            modelBuilder.Entity<VOTOS>()
                .Property(e => e.FACULTAD)
                .IsUnicode(false);

            modelBuilder.Entity<VOTOS>()
                .Property(e => e.NIVEL_ESTUDIO)
                .IsUnicode(false);

            modelBuilder.Entity<CEDULAS_APROBADAS>()
                .Property(e => e.CEDULA)
                .IsUnicode(false);

            modelBuilder.Entity<CEDULAS_APROBADAS>()
                .Property(e => e.NOMBRE)
                .IsUnicode(false);

            modelBuilder.Entity<CEDULAS_APROBADAS>()
                .Property(e => e.APELLIDO)
                .IsUnicode(false);

            modelBuilder.Entity<ELECCION>()
                .Property(e => e.TITULO)
                .IsUnicode(false);

            modelBuilder.Entity<ELECCION>()
                .Property(e => e.TIPO_INSTITUTO)
                .IsUnicode(false);

            modelBuilder.Entity<ELECCION>()
                .Property(e => e.NOMBRE_INSTITUTO)
                .IsUnicode(false);

            modelBuilder.Entity<ELECCION>()
                .Property(e => e.DESCRIPCION)
                .IsUnicode(false);

            modelBuilder.Entity<PREGUNTAS_SEGURIDAD>()
                .Property(e => e.PREGUNTA)
                .IsUnicode(false);

            modelBuilder.Entity<PREGUNTAS_SEGURIDAD>()
                .Property(e => e.RESPUESTA)
                .IsUnicode(false);
        }
    }
}
