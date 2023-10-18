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

    public virtual DbSet<MlTbUsuariosSolicitante> MlTbUsuariosSolicitantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=www.developers.upiicsa.ipn.mx;Database=db_CorreosInst_UPIICSA;User ID=correos_institucionales;Password=correos_institucionales_05;Trusted_Connection=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MlTbUsuariosSolicitante>(entity =>
        {
            entity.HasKey(e => e.IdUsuarioSolicitante);

            entity.ToTable("ML_tbUsuariosSolicitantes");

            entity.Property(e => e.UsuNombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("usuNombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
