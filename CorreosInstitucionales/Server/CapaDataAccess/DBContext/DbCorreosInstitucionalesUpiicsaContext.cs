using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class DbCorreosInstitucionalesUpiicsaContext : DbContext
{
    private readonly IConfiguration? _config;

    public DbCorreosInstitucionalesUpiicsaContext()
    {
    }

    public DbCorreosInstitucionalesUpiicsaContext(DbContextOptions<DbCorreosInstitucionalesUpiicsaContext> options, IConfiguration config)
        : base(options)
    {
        _config = config;
    }

    public virtual DbSet<McCatAreasDepto> McCatAreasDeptos { get; set; }

    public virtual DbSet<McCatCarrera> McCatCarreras { get; set; }

    public virtual DbSet<McCatEdificio> McCatEdificios { get; set; }

    public virtual DbSet<McCatEscuela> McCatEscuelas { get; set; }

    public virtual DbSet<McCatEstadosSolicitud> McCatEstadosSolicituds { get; set; }

    public virtual DbSet<McCatLink> McCatLinks { get; set; }

    public virtual DbSet<McCatNoExtension> McCatNoExtensions { get; set; }

    public virtual DbSet<McCatPiso> McCatPisos { get; set; }

    public virtual DbSet<McCatRole> McCatRoles { get; set; }

    public virtual DbSet<McCatTiposPersonal> McCatTiposPersonals { get; set; }

    public virtual DbSet<McCatTiposSolicitud> McCatTiposSolicituds { get; set; }

    public virtual DbSet<MpTbUsuario> MpTbUsuarios { get; set; }

    public virtual DbSet<MtTbSolicitudesTicket> MtTbSolicitudesTickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //=> optionsBuilder.UseSqlServer("Server=localhost;Database=db_CorreosInstitucionales_UPIICSA;Trusted_Connection=True;TrustServerCertificate=True;");
    => optionsBuilder.UseSqlServer(_config.GetConnectionString("SQLServer"));
    //=> optionsBuilder.UseSqlServer(_config.GetSection("ConnectionStrings:SQLServer").Value);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<McCatAreasDepto>(entity =>
        {
            entity.HasKey(e => e.IdAreaDepto).HasName("PK_MCE_catExtencionesAreas");

            entity.Property(e => e.IdAreaDepto).HasComment("PK ID Único del Área / Departamento");
            entity.Property(e => e.AreIdEdificio)
                .HasDefaultValue(1)
                .HasComment("FK ID del Edificio");
            entity.Property(e => e.AreIdPiso)
                .HasDefaultValue(1)
                .HasComment("FK ID del Piso");
            entity.Property(e => e.AreNoExtension).HasComment("Números de Extensión relacionados al Área / Departamento");
            entity.Property(e => e.AreNombreAreaDepto).HasComment("Nombre del Área / Departamento");
            entity.Property(e => e.AreStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
            entity.Property(e => e.AreTitular).HasComment("Titular responsable del Área / Departamento");

            entity.HasOne(d => d.AreIdEdificioNavigation).WithMany(p => p.McCatAreasDeptos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MC_catAreasDeptos_MC_catEdificios");

            entity.HasOne(d => d.AreIdPisoNavigation).WithMany(p => p.McCatAreasDeptos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MC_catAreasDeptos_MC_catPisos");
        });

        modelBuilder.Entity<McCatCarrera>(entity =>
        {
            entity.HasKey(e => e.IdCarrera).HasName("PK_MCE_catCarreras");

            entity.Property(e => e.IdCarrera).HasComment("PK ID Único de la Carrera");
            entity.Property(e => e.CarrClave)
                .HasDefaultValue("-")
                .HasComment("Clave de la Carrera / Licenciatura");
            entity.Property(e => e.CarrNombre).HasComment("Nombre de la Carrera / Licenciatura");
            entity.Property(e => e.CarrStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
        });

        modelBuilder.Entity<McCatEdificio>(entity =>
        {
            entity.HasKey(e => e.IdEdificio).HasName("PK_MCE_catEdificios");

            entity.Property(e => e.IdEdificio).HasComment("PK ID Único del Edificio");
            entity.Property(e => e.EdiNombreAlias).HasComment("Nombre Alias del Edificio");
            entity.Property(e => e.EdiNombreOficial).HasComment("Nombre Oficial del Edificio");
            entity.Property(e => e.EdiStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
        });

        modelBuilder.Entity<McCatEscuela>(entity =>
        {
            entity.HasKey(e => e.IdEscuela).HasName("PK_MCE_catEscuelas");

            entity.Property(e => e.IdEscuela).HasComment("PK ID Único de la Escuela");
            entity.Property(e => e.EscLogo).HasComment("Nombre de la Imágen del Logo");
            entity.Property(e => e.EscNoEscuela)
                .HasDefaultValue("-")
                .HasComment("Número de la Escuela");
            entity.Property(e => e.EscNombreCorto).HasComment("Nombre Corto de la Escuela");
            entity.Property(e => e.EscNombreLargo).HasComment("Nombre Largo de la Escuela");
            entity.Property(e => e.EscStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
        });

        modelBuilder.Entity<McCatEstadosSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdEstadoSolicitud).HasName("PK_MCE_catEstadosSolicitud");

            entity.Property(e => e.IdEstadoSolicitud).HasComment("PK ID Único del Estado de la Solicitud");
            entity.Property(e => e.EdosolDescripcion).HasComment("Detalle del Estado de la Solicitud");
            entity.Property(e => e.EdosolNombreEstado)
                .HasDefaultValue("-")
                .HasComment("Estado de la Solicitud (1 - Levantado, 2 - Pendiente, 3 - En Proceso, 4 - Atendido)");
        });

        modelBuilder.Entity<McCatLink>(entity =>
        {
            entity.HasKey(e => e.IdLink).HasName("PK_MCE_catLinks");

            entity.Property(e => e.IdLink).HasComment("PK ID Único del Enlace Link");
            entity.Property(e => e.LinkEnlace).HasComment("Enlace o Link");
            entity.Property(e => e.LinkNombre).HasComment("Nombre del Enlace o Link");
            entity.Property(e => e.LinkStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
        });

        modelBuilder.Entity<McCatNoExtension>(entity =>
        {
            entity.HasKey(e => e.IdExtension).HasName("PK_MCE_catExtensiones");

            entity.Property(e => e.IdExtension).HasComment("PK ID Único del Número de Extensión");
            entity.Property(e => e.ExtIdAreaDepto)
                .HasDefaultValue(1)
                .HasComment("FK ID del Área / Departamento");
            entity.Property(e => e.ExtNoExtension)
                .HasDefaultValueSql("((0))")
                .HasComment("Número de Extensión");
            entity.Property(e => e.ExtStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");

            entity.HasOne(d => d.ExtIdAreaDeptoNavigation).WithMany(p => p.McCatNoExtensions).HasConstraintName("FK_MC_catNoExtension_MC_catAreasDeptos");
        });

        modelBuilder.Entity<McCatPiso>(entity =>
        {
            entity.HasKey(e => e.IdPiso).HasName("PK_MCE_catPisos");

            entity.Property(e => e.IdPiso).HasComment("PK ID Único del Piso / Nivel");
            entity.Property(e => e.PisoDescripcion).HasComment("Nombre / Descripción del Piso");
            entity.Property(e => e.PisoStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
        });

        modelBuilder.Entity<McCatRole>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK_MCE_catRoles");

            entity.Property(e => e.IdRol).HasComment("PK ID Único del Rol");
            entity.Property(e => e.RolDescripcion).HasComment("Descripción detallada del Rol");
            entity.Property(e => e.RolNombre).HasComment("Nombre del Rol");
        });

        modelBuilder.Entity<McCatTiposPersonal>(entity =>
        {
            entity.HasKey(e => e.IdTipoPersonal).HasName("PK_MC_TipoPersonal");

            entity.Property(e => e.IdTipoPersonal).HasComment("PK ID Único del Tipo de Personal");
            entity.Property(e => e.TipoperDescripcion).HasComment("Descripción detallada del Tipo de Personal");
            entity.Property(e => e.TipoperNombre).HasComment("Nombre del Tipo de Personal");
            entity.Property(e => e.TipoperStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
        });

        modelBuilder.Entity<McCatTiposSolicitud>(entity =>
        {
            entity.HasKey(e => e.IdTipoSolicitud).HasName("PK_MC_catTipoSolicitud");

            entity.Property(e => e.IdTipoSolicitud).HasComment("PK ID Único del Tipo de Solicitud");
            entity.Property(e => e.TiposolDescripcion)
                .HasDefaultValue("-")
                .HasComment("Descripción breve del Tipo de Solicitud");
            entity.Property(e => e.TiposolStatus)
                .HasDefaultValue(true)
                .HasComment("Estado (1 = Activo, 0 = Inactivo)");
        });

        modelBuilder.Entity<MpTbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK_ML_tbUsuariosSolicitantes");

            entity.Property(e => e.IdUsuario).HasComment("PK ID Único del Usuario Solicitante o Administrador");
            entity.Property(e => e.UsuAñoEgreso)
                .HasDefaultValue(1900)
                .HasComment("Año de Egreso en caso de ser Alumno Egresado");
            entity.Property(e => e.UsuBoletaAlumno).HasComment("Número de Boleta del Usuario Alumno/Egresado");
            entity.Property(e => e.UsuBoletaMaestria).HasComment("Número de Boleta del Usuario Alumno de Maestría");
            entity.Property(e => e.UsuContraseña).HasComment("Contraseña Encriptada en la Plataforma SACI");
            entity.Property(e => e.UsuCorreoInstitucionalContraseña).HasComment("Contraseña del Correo Electrónico Institucional IPN");
            entity.Property(e => e.UsuCorreoInstitucionalCuenta).HasComment("Correo Electrónico Institucional IPN");
            entity.Property(e => e.UsuCorreoPersonalCuentaAnterior).HasComment("Correo Personal Anterior");
            entity.Property(e => e.UsuCorreoPersonalCuentaNueva).HasComment("Correo Personal Actual / Nuevo");
            entity.Property(e => e.UsuCurp).HasComment("CURP del Usuario");
            entity.Property(e => e.UsuFechaHoraAlta)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha Hora del Alta del Usuario");
            entity.Property(e => e.UsuFileNameComprobanteInscripcion).HasComment("Nombre del Archivo PDF de la Tira de Materias / Certificado de Calificaciones / SIP");
            entity.Property(e => e.UsuFileNameCurp).HasComment("Nombre del Archivo PDF del CURP del Usuario");
            entity.Property(e => e.UsuIdAreaDepto)
                .HasDefaultValue(1)
                .HasComment("FK ID del Área / Departamento domde labora el Empleado");
            entity.Property(e => e.UsuIdCarrera)
                .HasDefaultValue(1)
                .HasComment("FK ID de la Carrera que pertenece o cursó");
            entity.Property(e => e.UsuIdRol)
                .HasDefaultValue(2)
                .HasComment("FK ID Único del Rol { Administrador, Usuario Solicitante }");
            entity.Property(e => e.UsuIdTipoPersonal)
                .HasDefaultValue(1)
                .HasComment("FK ID del Tipo de Personal del Usuario Solicitante ([1 - Alumno Inscrito], [2 - Alumno Egresado], [3 - Maestria], [4 - Administrativo], [5 - Docente])");
            entity.Property(e => e.UsuNoCelularAnterior).HasComment("Número Celular Anterior del Usuario");
            entity.Property(e => e.UsuNoCelularNuevo).HasComment("Número Celular Actual/Nuevo del Usuario");
            entity.Property(e => e.UsuNoExtension).HasComment("Número de Extensión del Área / Departamento");
            entity.Property(e => e.UsuNombre)
                .HasDefaultValue("-")
                .HasComment("Nombre del Usuario Solicitante o Administrador");
            entity.Property(e => e.UsuNumeroEmpleado).HasComment("Número de Empleado del Usuario { Administrativo, Docente }");
            entity.Property(e => e.UsuPrimerApellido)
                .HasDefaultValue("-")
                .HasComment("Primer Apellido del Usuario");
            entity.Property(e => e.UsuRecuperarContraseña).HasComment("Bandera { 0 = Inicia Sesión, 1 = Pide cambiar contraseña temporal }");
            entity.Property(e => e.UsuSegundoApellido).HasComment("Segundo Apellido del Usuario");
            entity.Property(e => e.UsuSemestre)
                .HasDefaultValueSql("((1))")
                .HasComment("Semestre que cursa");
            entity.Property(e => e.UsuStatus)
                .HasDefaultValue(true)
                .HasComment("Estatus { 0 = Inactivo, 1 = Activo }");

            entity.HasOne(d => d.UsuIdAreaDeptoNavigation).WithMany(p => p.MpTbUsuarios).HasConstraintName("FK_MP_tbUsuarios_MC_catAreasDeptos");

            entity.HasOne(d => d.UsuIdCarreraNavigation).WithMany(p => p.MpTbUsuarios).HasConstraintName("FK_MP_tbUsuarios_MC_catCarreras");

            entity.HasOne(d => d.UsuIdRolNavigation).WithMany(p => p.MpTbUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MP_tbUsuarios_MC_catRoles");

            entity.HasOne(d => d.UsuIdTipoPersonalNavigation).WithMany(p => p.MpTbUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MP_tbUsuarios_MC_catTiposPersonal");
        });

        modelBuilder.Entity<MtTbSolicitudesTicket>(entity =>
        {
            entity.HasKey(e => e.IdSolicitudTicket).HasName("PK_MCE_tbSolicitud");

            entity.Property(e => e.IdSolicitudTicket).HasComment("PK ID Único de la Solicitud");
            entity.Property(e => e.SolCapturaCuentaBloqueada).HasComment("Nombre del Archivo PDF de la Captura de la Cuenta Bloqueada");
            entity.Property(e => e.SolCapturaError).HasComment("Nombre del Archivo PDF de la Captura del Error");
            entity.Property(e => e.SolCapturaEscaneoAntivirus).HasComment("Nombre del Archivo PDF de la Captura del Escaneo del Antivirus");
            entity.Property(e => e.SolEncuestaCalidadCalificacion).HasComment("Calificación de la Encuesta de Calidad con emojis caritas o estrellas.");
            entity.Property(e => e.SolEncuestaCalidadComentarios).HasComment("Comentarios, observaciones o notas por la atención de la Solicitud-Ticket.");
            entity.Property(e => e.SolFechaHoraCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha Hora de la creación de la Solicitud-Ticket.");
            entity.Property(e => e.SolFechaHoraEncuesta).HasComment("Fecha Hora de la respuesta a la Encuesta de Calidad de la Solicitud-Ticket.");
            entity.Property(e => e.SolIdEstadoSolicitud)
                .HasDefaultValue(1)
                .HasComment("FK ID del Estado de la Solicitud (1 = Alta/Levantad@, 2 = Pendiente, 3 = En Proceso, 4 = Atendido)");
            entity.Property(e => e.SolIdTipoSolicitud)
                .HasDefaultValue(1)
                .HasComment("FK ID del Tipo de Solicitud (1 - A, 2 - B, 3 - C, 4 -D, 5 - E, 6 - F, 7 - G, 8 - OTRO)");
            entity.Property(e => e.SolIdUsuario)
                .HasDefaultValue(1)
                .HasComment("FK ID del Usuario Solicitante");
            entity.Property(e => e.SolObservacionesSolicitud).HasComment("Observación y/o comentario del problema de la Solicitud-Ticket.");
            entity.Property(e => e.SolToken).HasComment("Magic Link con Token para Encuestra de Calidad");
            entity.Property(e => e.SolValidacionDatos).HasComment("Validación de Datos por el Administrador { 0 - No Validados, 1 - Validados }");

            entity.HasOne(d => d.SolIdEstadoSolicitudNavigation).WithMany(p => p.MtTbSolicitudesTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MT_tbSolicitudesTickets_MC_catEstadosSolicitud");

            entity.HasOne(d => d.SolIdTipoSolicitudNavigation).WithMany(p => p.MtTbSolicitudesTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MT_tbSolicitudesTickets_MC_catTiposSolicitud");

            entity.HasOne(d => d.SolIdUsuarioNavigation).WithMany(p => p.MtTbSolicitudesTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MT_tbSolicitudesTickets_MP_tbUsuarios");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
