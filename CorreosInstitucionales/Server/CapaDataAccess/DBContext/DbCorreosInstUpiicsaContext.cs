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

    public virtual DbSet<MceCatExtensione> MceCatExtensiones { get; set; }

    public virtual DbSet<MceCatLink> MceCatLinks { get; set; }

    public virtual DbSet<MceCatPiso> MceCatPisos { get; set; }

    public virtual DbSet<MceCatRole> MceCatRoles { get; set; }

    public virtual DbSet<MceCatTipoPersonal> MceCatTipoPersonals { get; set; }

    public virtual DbSet<MceCatTipoSolicitud> MceCatTipoSolicituds { get; set; }

    public virtual DbSet<MceTbSolicitudTicket> MceTbSolicitudTickets { get; set; }

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

            entity.HasOne(d => d.AreIdEdificioNavigation).WithMany(p => p.MceCatAreasDeptos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MCE_catAreasDeptos_MCE_catEdificios");

            entity.HasOne(d => d.AreIdPisoNavigation).WithMany(p => p.MceCatAreasDeptos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MCE_catAreasDeptos_MCE_catPisos");
        });

        modelBuilder.Entity<MceCatCarrera>(entity =>
        {
            entity.Property(e => e.CarrClave).HasDefaultValueSql("('-')");
            entity.Property(e => e.CarrStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatEdificio>(entity =>
        {
            entity.Property(e => e.EdiStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatEscuela>(entity =>
        {
            entity.Property(e => e.EscNoEscuela).HasDefaultValueSql("('-')");
            entity.Property(e => e.EscStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatEstadosSolicitud>(entity =>
        {
            entity.Property(e => e.EdosolNombreEstado).HasDefaultValueSql("('-')");
        });

        modelBuilder.Entity<MceCatExtensione>(entity =>
        {
            entity.Property(e => e.ExtIdAreaDepto).HasDefaultValueSql("((1))");
            entity.Property(e => e.ExtNoExtension).HasDefaultValueSql("((0))");
            entity.Property(e => e.ExtStatus).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.ExtIdAreaDeptoNavigation).WithMany(p => p.MceCatExtensiones).HasConstraintName("FK_MCE_catExtensiones_MCE_catAreasDeptos");
        });

        modelBuilder.Entity<MceCatLink>(entity =>
        {
            entity.Property(e => e.LinkStatus).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MceCatPiso>(entity =>
        {
            entity.Property(e => e.PisoStatus).HasDefaultValueSql("((1))");
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

        modelBuilder.Entity<MceTbSolicitudTicket>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudTicket).HasName("PK_MCE_tbSolicitud");

            entity.Property(e => e.SolFechaHoraCreacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.SolIdEstadoSolicitudNavigation).WithMany(p => p.MceTbSolicitudTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MCE_tbSolicitudTicket_MCE_catEstadosSolicitud");

            entity.HasOne(d => d.SolIdTipoSolicitudNavigation).WithMany(p => p.MceTbSolicitudTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MCE_tbSolicitudTicket_MCE_catTipoSolicitud");

            entity.HasOne(d => d.SolIdUsuarioNavigation).WithMany(p => p.MceTbSolicitudTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MCE_tbSolicitudTicket_MCE_tbUsuarios");
        });

        modelBuilder.Entity<MceTbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK_ML_tbUsuariosSolicitantes");

            entity.Property(e => e.IdUsuario).HasComment("ID Único del Usuario Solicitante o Administrador");
            entity.Property(e => e.UsuBoletaAlumno).HasComment("Numero de Boleta del Uusario Solicitante");
            entity.Property(e => e.UsuContraseña).HasComment("Contraseña del Usuario Solicitante");
            entity.Property(e => e.UsuFechaHoraAlta).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UsuIdRol).HasDefaultValueSql("((2))");
            entity.Property(e => e.UsuIdTipoPersonal).HasComment("Tipo de Personal del Usuario Solicitante");
            entity.Property(e => e.UsuNombre).HasComment("Nombre del Usuario Solicitante");
            entity.Property(e => e.UsuNumeroEmpleado).HasComment("Numero del Empleado del Usuario Solicitante");
            entity.Property(e => e.UsuPrimerApellido)
                .HasDefaultValueSql("('-')")
                .HasComment("Primer apellido del Usuario Solicitante");
            entity.Property(e => e.UsuRecuperarContraseña)
                .HasDefaultValueSql("((0))")
                .HasComment("Contraseña Temporal que se le proporciona al Usuario Solicitante");
            entity.Property(e => e.UsuSegundoApellido).HasComment("Segundo Apellido del Usuario Solicitante");
            entity.Property(e => e.UsuStatus)
                .HasDefaultValueSql("((1))")
                .HasComment("Activo / Inactivo");

            entity.HasOne(d => d.UsuIdAreaDeptoNavigation).WithMany(p => p.MceTbUsuarios).HasConstraintName("FK_MCE_tbUsuarios_MCE_catAreasDeptos");

            entity.HasOne(d => d.UsuIdCarreraNavigation).WithMany(p => p.MceTbUsuarios).HasConstraintName("FK_MCE_tbUsuarios_MCE_catCarreras");

            entity.HasOne(d => d.UsuIdRolNavigation).WithMany(p => p.MceTbUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MCE_tbUsuarios_MCE_catRoles");

            entity.HasOne(d => d.UsuIdTipoPersonalNavigation).WithMany(p => p.MceTbUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MCE_tbUsuarios_MCE_catTipoPersonal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
