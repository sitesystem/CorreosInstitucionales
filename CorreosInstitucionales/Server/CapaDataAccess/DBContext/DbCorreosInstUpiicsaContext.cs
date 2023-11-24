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

    public virtual DbSet<MceCatEscuela> MceCatEscuelas { get; set; }

    public virtual DbSet<MceCatEstadosSolicitud> MceCatEstadosSolicituds { get; set; }

    public virtual DbSet<MceCatLink> MceCatLinks { get; set; }

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

            entity.Property(e => e.AreIdEdificio).HasDefaultValueSql("((1))");
            entity.Property(e => e.AreIdPiso).HasDefaultValueSql("((1))");
            entity.Property(e => e.AreStatus).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.AreIdEdificioNavigation).WithMany(p => p.MceCatAreasDeptos).HasConstraintName("FK_MCE_catAreasDeptos_MCE_catEdificios");

            entity.HasOne(d => d.AreIdPisoNavigation).WithMany(p => p.MceCatAreasDeptos).HasConstraintName("FK_MCE_catAreasDeptos_MCE_catPisos");
        });

        modelBuilder.Entity<MceCatCarrera>(entity =>
        {
            entity.Property(e => e.CarrStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatEdificio>(entity =>
        {
            entity.Property(e => e.EdiStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatEscuela>(entity =>
        {
            entity.Property(e => e.EscStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatLink>(entity =>
        {
            entity.Property(e => e.LinkStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatTipoPersonal>(entity =>
        {
            entity.HasKey(e => e.IdTipoPersonal).HasName("PK_MC_TipoPersonal");

            entity.Property(e => e.TipoperStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatTipoSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdTipoSolicitud).HasName("PK_MC_catTipoSolicitud");

            entity.Property(e => e.IdTipoSolicitud).HasComment("ID del tipo de solicitud");
            entity.Property(e => e.TiposolDescripcion)
                .HasDefaultValueSql("('-')")
                .HasComment("Descripcion del tipo de la solicitud");
            entity.Property(e => e.TiposolStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceTbSolicitud>(entity =>
        {
            entity.Property(e => e.IdSolicitud).ValueGeneratedNever();
        });

        modelBuilder.Entity<MceTbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioSolicitante).HasName("PK_ML_tbUsuariosSolicitantes");

            entity.Property(e => e.IdUsuarioSolicitante).HasComment("Descripcion del Usuario Solicitante");
            entity.Property(e => e.UsuBoletaAlumno).HasComment("Numero de Boleta del Uusario Solicitante");
            entity.Property(e => e.UsuContraseña).HasComment("Contraseña del Usuario Solicitante");
            entity.Property(e => e.UsuFechaHoraAlta).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UsuIdTipoPersonal).HasComment("Tipo de Personal del Usuario Solicitante");
            entity.Property(e => e.UsuNombre).HasComment("Nombre del Usuario Solicitante");
            entity.Property(e => e.UsuNumeroEmpleado).HasComment("Numero del Empleado del Usuario Solicitante");
            entity.Property(e => e.UsuPrimerApellido)
                .HasDefaultValueSql("('-')")
                .HasComment("Primer apellido del Usuario Solicitante");
            entity.Property(e => e.UsuRecuperarContraseñas).HasComment("Contraseña Temporal que se le proporciona al Usuario Solicitante");
            entity.Property(e => e.UsuSegundoApellido).HasComment("Segundo Apellido del Usuario Solicitante");
            entity.Property(e => e.UsuStatus)
                .HasDefaultValueSql("((1))")
                .HasComment("Activo / Inactivo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
