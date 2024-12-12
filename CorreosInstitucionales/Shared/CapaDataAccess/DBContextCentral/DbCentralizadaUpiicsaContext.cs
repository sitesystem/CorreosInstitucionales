using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

public partial class DbCentralizadaUpiicsaContext : DbContext
{
    public DbCentralizadaUpiicsaContext()
    {
    }

    public DbCentralizadaUpiicsaContext(DbContextOptions<DbCentralizadaUpiicsaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CatArchivosUsuario> CatArchivosUsuarios { get; set; }

    public virtual DbSet<CatAreasDepto> CatAreasDeptos { get; set; }

    public virtual DbSet<CatCarrera> CatCarreras { get; set; }

    public virtual DbSet<CatEdificio> CatEdificios { get; set; }

    public virtual DbSet<CatEntidadesFederativa> CatEntidadesFederativas { get; set; }

    public virtual DbSet<CatEscuela> CatEscuelas { get; set; }

    public virtual DbSet<CatExtension> CatExtensions { get; set; }

    public virtual DbSet<CatLink> CatLinks { get; set; }

    public virtual DbSet<CatPiso> CatPisos { get; set; }

    public virtual DbSet<CatRole> CatRoles { get; set; }

    public virtual DbSet<CatSemestre> CatSemestres { get; set; }

    public virtual DbSet<CatTiposPersonalUdi> CatTiposPersonalUdis { get; set; }

    public virtual DbSet<CatTiposUsuario> CatTiposUsuarios { get; set; }

    public virtual DbSet<RelAreasExtensione> RelAreasExtensiones { get; set; }

    public virtual DbSet<TbAlumno> TbAlumnos { get; set; }

    public virtual DbSet<TbDireccione> TbDirecciones { get; set; }

    public virtual DbSet<TbEmpleado> TbEmpleados { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=148.204.211.186;Database=db_Centralizada_UPIICSA;User ID=sa;Password=#Password!!1;Trusted_Connection=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatArchivosUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoArchivo).HasName("PK__catArchi__2678FEFD902F410D");

            entity.Property(e => e.IdTipoArchivo).ValueGeneratedNever();
        });

        modelBuilder.Entity<CatAreasDepto>(entity =>
        {
            entity.HasKey(e => e.IdAreaDepto).HasName("PK_IdAreaDepto");

            entity.Property(e => e.IdAreaDepto).HasComment("PK ID Único del Área/Departamento");
            entity.Property(e => e.AreCorreoAreaDepto).HasComment("Correo Electrónico Administrativo del Área/Departamento");
            entity.Property(e => e.AreIdEdificio).HasComment("FK ID Foránea del Edificio");
            entity.Property(e => e.AreIdPiso).HasComment("FK ID Foránea del Piso");
            entity.Property(e => e.AreNombreAreaDepto).HasComment("Nombre del Área/Departamento");
            entity.Property(e => e.AreNombreJefe).HasComment("Jefe(a) del Área/Departamento");
            entity.Property(e => e.AreStatus)
                .HasDefaultValue(true)
                .HasComment("Estatus del Área/Departamento { 1 = Está en función, 0 = No está en función }");
            entity.Property(e => e.AreTitularResponsable).HasComment("Titular responsable del Área/Departamento");

            entity.HasOne(d => d.AreIdEdificioNavigation).WithMany(p => p.CatAreasDeptos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_areIdEdificio");

            entity.HasOne(d => d.AreIdPisoNavigation).WithMany(p => p.CatAreasDeptos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_areIdPiso");
        });

        modelBuilder.Entity<CatCarrera>(entity =>
        {
            entity.Property(e => e.IdCarrera).HasComment("PK ID Único de la Carrera");
            entity.Property(e => e.CarrCarreraEspecialidad).HasComment("Nombre de la Carrera / Especialidad");
            entity.Property(e => e.CarrClave).HasComment("Clave o Código de la Carrera");
        });

        modelBuilder.Entity<CatEdificio>(entity =>
        {
            entity.HasKey(e => e.IdEdificio).HasName("PK__catEdifi__92220CB04BB4B8A3");
        });

        modelBuilder.Entity<CatEntidadesFederativa>(entity =>
        {
            entity.HasKey(e => e.IdEntidadFederativa).HasName("PK__catEntid__26F25BCE5618A75F");
        });

        modelBuilder.Entity<CatEscuela>(entity =>
        {
            entity.HasKey(e => e.IdEscuela).HasName("PK__catEscue__D6C6BBF554D52C52");
        });

        modelBuilder.Entity<CatExtension>(entity =>
        {
            entity.HasKey(e => e.IdExtension).HasName("PK__catExten__103C0F065685F9FE");
        });

        modelBuilder.Entity<CatLink>(entity =>
        {
            entity.HasKey(e => e.IdLink).HasName("PK__catLinks__31D8A6ED27F2C777");
        });

        modelBuilder.Entity<CatPiso>(entity =>
        {
            entity.HasKey(e => e.IdPiso).HasName("PK__catPisos__F2823D8AB83E8068");
        });

        modelBuilder.Entity<CatRole>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__catRoles__2A49584CB148E2B2");
        });

        modelBuilder.Entity<CatSemestre>(entity =>
        {
            entity.Property(e => e.SemNivelSemestre).HasComment("Último Semestre Cursado del Alumn@");
        });

        modelBuilder.Entity<CatTiposPersonalUdi>(entity =>
        {
            entity.HasKey(e => e.IdRolUdi).HasName("PK__catRoles__A074A3C4934CAE40");
        });

        modelBuilder.Entity<CatTiposUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("PK__catTipos__CA04062BEBDA6F7F");
        });

        modelBuilder.Entity<RelAreasExtensione>(entity =>
        {
            entity.HasKey(e => e.IdAreaNumerosExtensiones).HasName("PK__tbRelaci__D95A48F745B85111");

            entity.ToTable("relAreasExtensiones", tb => tb.HasComment(""));

            entity.HasOne(d => d.NumIdAreaDeptoNavigation).WithMany(p => p.RelAreasExtensiones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_numIdAreaDepto");

            entity.HasOne(d => d.NumIdExtensionNavigation).WithMany(p => p.RelAreasExtensiones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_numIdExtension");
        });

        modelBuilder.Entity<TbAlumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno).HasName("PK__tbAlumno__43FBBAC763DDBA1B");

            entity.HasOne(d => d.AluIdCarreraNavigation).WithMany(p => p.TbAlumnos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdCarrera");

            entity.HasOne(d => d.AluIdSemestreNavigation).WithMany(p => p.TbAlumnos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdSemestre");

            entity.HasOne(d => d.AluIdUsuarioNavigation).WithMany(p => p.TbAlumnos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IdUsuario");
        });

        modelBuilder.Entity<TbDireccione>(entity =>
        {
            entity.HasKey(e => e.IdDireccion).HasName("PK__tbDirecc__1F8E0C76F3409FFC");

            entity.HasOne(d => d.DirIdEntidadFederativaNavigation).WithMany(p => p.TbDirecciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_dirIdEntidadFederativa");
        });

        modelBuilder.Entity<TbEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__tbEmplea__5295297C9717EB29");

            entity.HasOne(d => d.EmpIdAreaDeptoNavigation).WithMany(p => p.TbEmpleados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_empIdAreaDepto");

            entity.HasOne(d => d.EmpIdUsuarioNavigation).WithMany(p => p.TbEmpleados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_empIdUsuario");
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.Property(e => e.IdUsuario).HasComment("PK ID Único del Usuario");
            entity.Property(e => e.UsuContraseniaPlataformas).HasComment("Contraseña con la que se ingresará a las Plataformas / Aplicaciones");
            entity.Property(e => e.UsuCorreoPersonalActual).HasComment("Correo Personal Actual / Nuevo");
            entity.Property(e => e.UsuCorreoPersonalAnterior).HasComment("Correo Personal Anterior por cambio o pérdida");
            entity.Property(e => e.UsuCurp).HasComment("CURP del Usuari@");
            entity.Property(e => e.UsuEdad).HasComment("Edad del Usuari@");
            entity.Property(e => e.UsuEstadoCivil).HasComment("Estado Civil del Usuari@");
            entity.Property(e => e.UsuFechaHoraActualizacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UsuFechaHoraAlta)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha y Hora en que se dió de alta al Usuari@");
            entity.Property(e => e.UsuFechaNacimiento).HasComment("Fecha de Nacimiento");
            entity.Property(e => e.UsuFotoPerfil).HasComment("Foto / Imágen de Perfil en formato .jpg / .png");
            entity.Property(e => e.UsuGenero).HasComment("Género / Sexo del Usuari@");
            entity.Property(e => e.UsuNacionalidad).HasComment("Nacionalidad del Usuari@");
            entity.Property(e => e.UsuNoCelularActual).HasComment("Número de Celular Actual / Nuevo");
            entity.Property(e => e.UsuNoCelularAnterior).HasComment("Número de Celular Anterior por cambio o pérdida");
            entity.Property(e => e.UsuNombres).HasComment("Nombre(s) del Usuari@");
            entity.Property(e => e.UsuPrimerApellido).HasComment("Primer Apellido del Usuari@");
            entity.Property(e => e.UsuRecuperacionContrasenia).HasComment("0 = Ingresa a la Plataforma, 1 = Pide ingresar una nueva contraseña y sale de la Aplicación");
            entity.Property(e => e.UsuSegundoApellido).HasComment("Segundo Apellido del Usuari@");
            entity.Property(e => e.UsuStatus)
                .HasDefaultValue(true)
                .HasComment("Estado { 1 = Activo, 0 = Inactivo }");

            entity.HasOne(d => d.UsuIdDireccionNavigation).WithMany(p => p.TbUsuarios).HasConstraintName("FK_usuIdDireccion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
