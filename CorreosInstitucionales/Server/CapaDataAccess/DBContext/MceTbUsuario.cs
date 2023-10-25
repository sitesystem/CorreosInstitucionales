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
    /// Descripcion del Usuario Solicitante
    /// </summary>
    [Key]
    public int IdUsuarioSolicitante { get; set; }

    /// <summary>
    /// Nombre del Usuario Solicitante
    /// </summary>
    [Column("usuNombre")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuNombre { get; set; }

    /// <summary>
    /// Primer apellido del Usuario Solicitante
    /// </summary>
    [Column("usuPrimerApellido")]
    [StringLength(150)]
    [Unicode(false)]
    public string? UsuPrimerApellido { get; set; }

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
    public string? UsuCurp { get; set; }

    [Column("usuFilenameCURP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuFilenameCurp { get; set; }

    [Column("usuNoCelular")]
    [StringLength(50)]
    [Unicode(false)]
    public string? UsuNoCelular { get; set; }

    /// <summary>
    /// Numero de Boleta del Uusario Solicitante
    /// </summary>
    [Column("usuBoleta")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuBoleta { get; set; }

    [Column("usuSemestre")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuSemestre { get; set; }

    [Column("usuAñoEgreso")]
    public int? UsuAñoEgreso { get; set; }

    /// <summary>
    /// Numero del Empleado del Usuario Solicitante
    /// </summary>
    [Column("usuNumeroEmpleado")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuNumeroEmpleado { get; set; }

    [Column("usuCorreoPersonal")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoPersonal { get; set; }

    /// <summary>
    /// Contraseña del Usuario Solicitante
    /// </summary>
    [Column("usuContraseñaPersonal")]
    [StringLength(300)]
    [Unicode(false)]
    public string? UsuContraseñaPersonal { get; set; }

    /// <summary>
    /// Contraseña Temporal que se le proporciona al Usuario Solicitante
    /// </summary>
    [Column("usuRecuperarContraseñas")]
    public bool? UsuRecuperarContraseñas { get; set; }

    /// <summary>
    /// Tipo de Personal del Usuario Solicitante
    /// </summary>
    [Column("usuIdTipoPersonal")]
    public int? UsuIdTipoPersonal { get; set; }

    [Column("usuIdRol")]
    public int? UsuIdRol { get; set; }

    [Column("usuIdCarrera")]
    public int? UsuIdCarrera { get; set; }

    [Column("usuCorreroInstitucional")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreroInstitucional { get; set; }

    [Column("usuContraseñaInstitucional")]
    [StringLength(300)]
    [Unicode(false)]
    public string? UsuContraseñaInstitucional { get; set; }

    [Column("usuFechaHoraAlta", TypeName = "datetime")]
    public DateTime? UsuFechaHoraAlta { get; set; }

    [Column("usuArchivoCompInscripcion")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuArchivoCompInscripcion { get; set; }

    [Column("usuArchivoCapturaEscaneo")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuArchivoCapturaEscaneo { get; set; }

    [Column("usuArchivoCapturaError")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuArchivoCapturaError { get; set; }

    /// <summary>
    /// Activo / Inactivo
    /// </summary>
    [Column("usuStatus")]
    public bool? UsuStatus { get; set; }

    [ForeignKey("UsuIdCarrera")]
    [InverseProperty("MceTbUsuarios")]
    public virtual MceCatCarrera? UsuIdCarreraNavigation { get; set; }

    [ForeignKey("UsuIdRol")]
    [InverseProperty("MceTbUsuarios")]
    public virtual MceCatRole? UsuIdRolNavigation { get; set; }

    [ForeignKey("UsuIdTipoPersonal")]
    [InverseProperty("MceTbUsuarios")]
    public virtual MceCatTipoPersonal? UsuIdTipoPersonalNavigation { get; set; }
}
