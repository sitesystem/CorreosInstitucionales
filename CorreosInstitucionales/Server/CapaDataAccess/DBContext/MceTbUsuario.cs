using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_tbUsuarios")]
public partial class MceTbUsuario
{
    /// <summary>
    /// ID Único del Usuario Solicitante o Administrador
    /// </summary>
    [Key]
    public int IdUsuario { get; set; }

    [Column("usuIdRol")]
    public int UsuIdRol { get; set; }

    /// <summary>
    /// Tipo de Personal del Usuario Solicitante
    /// </summary>
    [Column("usuIdTipoPersonal")]
    public int UsuIdTipoPersonal { get; set; }

    /// <summary>
    /// Nombre del Usuario Solicitante
    /// </summary>
    [Column("usuNombre")]
    [StringLength(200)]
    [Unicode(false)]
    public string UsuNombre { get; set; } = null!;

    /// <summary>
    /// Primer apellido del Usuario Solicitante
    /// </summary>
    [Column("usuPrimerApellido")]
    [StringLength(150)]
    [Unicode(false)]
    public string UsuPrimerApellido { get; set; } = null!;

    /// <summary>
    /// Segundo Apellido del Usuario Solicitante
    /// </summary>
    [Column("usuSegundoApellido")]
    [StringLength(150)]
    [Unicode(false)]
    public string? UsuSegundoApellido { get; set; }

    [Column("usuCURP")]
    [StringLength(18)]
    [Unicode(false)]
    public string UsuCurp { get; set; } = null!;

    [Column("usuFileNameCURP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuFileNameCurp { get; set; }

    [Column("usuNoCelularAnterior")]
    [StringLength(20)]
    [Unicode(false)]
    public string? UsuNoCelularAnterior { get; set; }

    [Column("usuNoCelularNuevo")]
    [StringLength(20)]
    [Unicode(false)]
    public string UsuNoCelularNuevo { get; set; } = null!;

    /// <summary>
    /// Numero de Boleta del Uusario Solicitante
    /// </summary>
    [Column("usuBoletaAlumno")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuBoletaAlumno { get; set; }

    [Column("usuBoletaMaestria")]
    [StringLength(7)]
    [Unicode(false)]
    public string? UsuBoletaMaestria { get; set; }

    [Column("usuIdCarrera")]
    public int? UsuIdCarrera { get; set; }

    [Column("usuSemestre")]
    [StringLength(15)]
    [Unicode(false)]
    public string? UsuSemestre { get; set; }

    [Column("usuAñoEgreso")]
    public int? UsuAñoEgreso { get; set; }

    [Column("usuFileNameComprobanteInscripcion")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuFileNameComprobanteInscripcion { get; set; }

    /// <summary>
    /// Numero del Empleado del Usuario Solicitante
    /// </summary>
    [Column("usuNumeroEmpleado")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuNumeroEmpleado { get; set; }

    [Column("usuIdAreaDepto")]
    public int? UsuIdAreaDepto { get; set; }

    [Column("usuNoExtension")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuNoExtension { get; set; }

    [Column("usuCorreoPersonalCuentaAnterior")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoPersonalCuentaAnterior { get; set; }

    [Column("usuCorreoPersonalCuentaNueva")]
    [StringLength(100)]
    [Unicode(false)]
    public string UsuCorreoPersonalCuentaNueva { get; set; } = null!;

    /// <summary>
    /// Contraseña del Usuario Solicitante
    /// </summary>
    [Column("usuContraseña")]
    [StringLength(300)]
    [Unicode(false)]
    public string UsuContraseña { get; set; } = null!;

    /// <summary>
    /// Contraseña Temporal que se le proporciona al Usuario Solicitante
    /// </summary>
    [Column("usuRecuperarContraseña")]
    public bool? UsuRecuperarContraseña { get; set; }

    [Column("usuCorreoInstitucionalCuenta")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoInstitucionalCuenta { get; set; }

    [Column("usuCorreoInstitucionalContraseña")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoInstitucionalContraseña { get; set; }

    [Column("usuFechaHoraAlta", TypeName = "datetime")]
    public DateTime UsuFechaHoraAlta { get; set; }

    /// <summary>
    /// Activo / Inactivo
    /// </summary>
    [Required]
    [Column("usuStatus")]
    public bool? UsuStatus { get; set; }

    [InverseProperty("SolIdUsuarioNavigation")]
    public virtual ICollection<MceTbSolicitudTicket> MceTbSolicitudTickets { get; set; } = new List<MceTbSolicitudTicket>();

    [ForeignKey("UsuIdAreaDepto")]
    [InverseProperty("MceTbUsuarios")]
    public virtual MceCatAreasDepto? UsuIdAreaDeptoNavigation { get; set; }

    [ForeignKey("UsuIdCarrera")]
    [InverseProperty("MceTbUsuarios")]
    public virtual MceCatCarrera? UsuIdCarreraNavigation { get; set; }

    [ForeignKey("UsuIdRol")]
    [InverseProperty("MceTbUsuarios")]
    public virtual MceCatRole UsuIdRolNavigation { get; set; } = null!;

    [ForeignKey("UsuIdTipoPersonal")]
    [InverseProperty("MceTbUsuarios")]
    public virtual MceCatTipoPersonal UsuIdTipoPersonalNavigation { get; set; } = null!;
}
