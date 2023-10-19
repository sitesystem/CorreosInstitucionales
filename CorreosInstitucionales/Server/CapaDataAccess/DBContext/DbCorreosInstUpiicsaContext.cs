using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class DbCorreosInstUpiicsaContext : DbContext
{
    public DbCorreosInstUpiicsaContext()
    {
    }

    public DbCorreosInstUpiicsaContext(DbContextOptions<DbCorreosInstUpiicsaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MceCatAreasDepto> MceCatAreasDeptos { get; set; }

    public virtual DbSet<MceCatCarrera> MceCatCarreras { get; set; }

    public virtual DbSet<MceCatEdificio> MceCatEdificios { get; set; }

    public virtual DbSet<MceCatEstadosSolicitud> MceCatEstadosSolicituds { get; set; }

    public virtual DbSet<MceCatPiso> MceCatPisos { get; set; }

    public virtual DbSet<MceCatRole> MceCatRoles { get; set; }

    public virtual DbSet<MceCatTipoPersonal> MceCatTipoPersonals { get; set; }

    public virtual DbSet<MceCatTipoSolicitud> MceCatTipoSolicituds { get; set; }

    public virtual DbSet<MceTbSolicitud> MceTbSolicituds { get; set; }

    public virtual DbSet<MceTbUsuario> MceTbUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=www.developers.upiicsa.ipn.mx;Database=db_CorreosInst_UPIICSA;User ID=correos_institucionales;Password=correos_institucionales_05;Trusted_Connection=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MceCatAreasDepto>(entity =>
        {
            entity.HasKey(e => e.IdAreaDepto).HasName("PK_MCE_catExtencionesAreas");

            entity.ToTable("MCE_catAreasDeptos");

            entity.Property(e => e.IdAreaDepto).HasColumnName("Id_AreaDepto");
            entity.Property(e => e.AreExtension)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("areExtension");
            entity.Property(e => e.AreIdEdificio).HasColumnName("areIdEdificio");
            entity.Property(e => e.AreIdPiso).HasColumnName("areIdPiso");
            entity.Property(e => e.AreNombre)
                .HasColumnType("text")
                .HasColumnName("areNombre");
            entity.Property(e => e.AreStatus).HasColumnName("areStatus");
        });

        modelBuilder.Entity<MceCatCarrera>(entity =>
        {
            entity.HasKey(e => e.IdCarrera);

            entity.ToTable("MCE_catCarreras");

            entity.Property(e => e.IdCarrera).HasColumnName("Id_Carrera");
            entity.Property(e => e.CarrNombre)
                .HasMaxLength(300)
                .HasColumnName("carrNombre");
            entity.Property(e => e.CarrStatus).HasColumnName("carrStatus");
        });

        modelBuilder.Entity<MceCatEdificio>(entity =>
        {
            entity.HasKey(e => e.IdEdificio);

            entity.ToTable("MCE_catEdificios");

            entity.Property(e => e.IdEdificio).ValueGeneratedNever();
            entity.Property(e => e.EdiNombreAlias)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ediNombreAlias");
            entity.Property(e => e.EdiNombreOficial)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ediNombreOficial");
            entity.Property(e => e.EdiStatus).HasColumnName("ediStatus");
        });

        modelBuilder.Entity<MceCatEstadosSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdEstadosSolicitud);

            entity.ToTable("MCE_catEstadosSolicitud");

            entity.Property(e => e.EstsolNombre)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("estsolNombre");
            entity.Property(e => e.PisoStatus).HasColumnName("pisoStatus");
        });

        modelBuilder.Entity<MceCatPiso>(entity =>
        {
            entity.HasKey(e => e.IdPiso);

            entity.ToTable("MCE_catPisos");

            entity.Property(e => e.PisoDescripcion)
                .HasMaxLength(50)
                .HasColumnName("pisoDescripcion");
            entity.Property(e => e.PisoStatus).HasColumnName("pisoStatus");
        });

        modelBuilder.Entity<MceCatRole>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("MCE_catRoles");

            entity.Property(e => e.RolDescripcion)
                .HasColumnType("text")
                .HasColumnName("rolDescripcion");
            entity.Property(e => e.RolNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("rolNombre");
        });

        modelBuilder.Entity<MceCatTipoPersonal>(entity =>
        {
            entity.HasKey(e => e.IdTipoPersonal).HasName("PK_MC_TipoPersonal");

            entity.ToTable("MCE_catTipoPersonal");

            entity.Property(e => e.IdTipoPersonal).HasColumnName("Id_Tipo_personal");
            entity.Property(e => e.TipoperDescripcion)
                .HasColumnType("text")
                .HasColumnName("tipoperDescripcion");
            entity.Property(e => e.TipoperNombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipoperNombre");
            entity.Property(e => e.TipoperStatus).HasColumnName("tipoperStatus");
        });

        modelBuilder.Entity<MceCatTipoSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdTipoSolicitud).HasName("PK_MC_catTipoSolicitud");

            entity.ToTable("MCE_catTipoSolicitud");

            entity.Property(e => e.IdTipoSolicitud).HasComment("ID del tipo de solicitud");
            entity.Property(e => e.TiposolDescripcion)
                .HasDefaultValueSql("(N'-')")
                .HasComment("Descripcion del tipo de la solicitud")
                .HasColumnType("text")
                .HasColumnName("tiposolDescripcion");
            entity.Property(e => e.TiposolStatus).HasColumnName("tiposolStatus");
        });

        modelBuilder.Entity<MceTbSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdSolicitud);

            entity.ToTable("MCE_tbSolicitud");

            entity.Property(e => e.IdSolicitud)
                .ValueGeneratedNever()
                .HasColumnName("Id_Solicitud");
            entity.Property(e => e.SolFecha)
                .HasColumnType("datetime")
                .HasColumnName("solFecha");
            entity.Property(e => e.SolIdAreaDepto).HasColumnName("solIdAreaDepto");
            entity.Property(e => e.SolIdEstadosSolicitud).HasColumnName("solIdEstadosSolicitud");
            entity.Property(e => e.SolIdTipoSolicitud).HasColumnName("solIdTipoSolicitud");
            entity.Property(e => e.SolIdUsuario).HasColumnName("solIdUsuario");
        });

        modelBuilder.Entity<MceTbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioSolicitante).HasName("PK_ML_tbUsuariosSolicitantes");

            entity.ToTable("MCE_tbUsuarios");

            entity.Property(e => e.IdUsuarioSolicitante).HasComment("Descripcion del Usuario Solicitante");
            entity.Property(e => e.UsuBoleta)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Numero de Boleta del Uusario Solicitante")
                .HasColumnName("usuBoleta");
            entity.Property(e => e.UsuContraseña)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasComment("Contraseña del Usuario Solicitante")
                .HasColumnName("usuContraseña");
            entity.Property(e => e.UsuContraseñaInstitucional)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("usuContraseñaInstitucional");
            entity.Property(e => e.UsuCorreoPersonal)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usuCorreoPersonal");
            entity.Property(e => e.UsuCorreroInstitucional)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usuCorreroInstitucional");
            entity.Property(e => e.UsuIdCarrera).HasColumnName("usuIdCarrera");
            entity.Property(e => e.UsuIdRol).HasColumnName("usuIdRol");
            entity.Property(e => e.UsuIdTipoPersonal)
                .HasComment("Tipo de Personal del Usuario Solicitante")
                .HasColumnName("usuIdTipoPersonal");
            entity.Property(e => e.UsuNombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasComment("Nombre del Usuario Solicitante")
                .HasColumnName("usuNombre");
            entity.Property(e => e.UsuNumeroEmpleado)
                .HasComment("Numero del Empleado del Usuario Solicitante")
                .HasColumnName("usuNumeroEmpleado");
            entity.Property(e => e.UsuPrimerApellido)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasDefaultValueSql("('-')")
                .HasComment("Primer apellido del Usuario Solicitante")
                .HasColumnName("usuPrimerApellido");
            entity.Property(e => e.UsuRecuperarContraseñas)
                .HasComment("Contraseña Temporal que se le proporciona al Usuario Solicitante")
                .HasColumnName("usuRecuperarContraseñas");
            entity.Property(e => e.UsuSegundoApellido)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasComment("Segundo Apellido del Usuario Solicitante")
                .HasColumnName("usuSegundoApellido");
            entity.Property(e => e.UsuStatus)
                .HasDefaultValueSql("((1))")
                .HasComment("Activo / Inactivo")
                .HasColumnName("usuStatus");

            entity.HasOne(d => d.UsuIdTipoPersonalNavigation).WithMany(p => p.MceTbUsuarios)
                .HasForeignKey(d => d.UsuIdTipoPersonal)
                .HasConstraintName("FK_ML_tbUsuariosSolicitantes_MC_TipoPersonal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
