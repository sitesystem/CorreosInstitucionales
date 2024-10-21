using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

[Table("MP_tbUsuarios")]
public partial class MpTbUsuario
{
    /// <summary>
    /// PK ID Único del Usuario Solicitante o Administrador
    /// </summary>
    [Key]
    public int IdUsuario { get; set; }

    /// <summary>
    /// FK ID Único del Rol { Administrador, Usuario Solicitante }
    /// </summary>
    [Column("usuIdRol")]
    public int UsuIdRol { get; set; }

    /// <summary>
    /// FK ID del Tipo de Personal del Usuario Solicitante ([1 - Alumno Inscrito], [2 - Alumno Egresado], [3 - Maestria], [4 - Administrativo], [5 - Docente])
    /// </summary>
    [Column("usuIdTipoPersonal")]
    public int UsuIdTipoPersonal { get; set; }

    /// <summary>
    /// Nombre(s) del Usuario Solicitante o Administrador
    /// </summary>
    [Column("usuNombres")]
    [StringLength(200)]
    [Unicode(false)]
    public string UsuNombres { get; set; } = null!;

    /// <summary>
    /// Primer Apellido del Usuario
    /// </summary>
    [Column("usuPrimerApellido")]
    [StringLength(150)]
    [Unicode(false)]
    public string UsuPrimerApellido { get; set; } = null!;

    /// <summary>
    /// Segundo Apellido del Usuario
    /// </summary>
    [Column("usuSegundoApellido")]
    [StringLength(150)]
    [Unicode(false)]
    public string? UsuSegundoApellido { get; set; }

    /// <summary>
    /// CURP del Usuario
    /// </summary>
    [Column("usuCURP")]
    [StringLength(18)]
    [Unicode(false)]
    public string UsuCurp { get; set; } = null!;

    /// <summary>
    /// Nombre del Archivo PDF del CURP del Usuario
    /// </summary>
    [Column("usuFileNameCURP")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuFileNameCurp { get; set; }

    /// <summary>
    /// Número Celular Anterior del Usuario
    /// </summary>
    [Column("usuNoCelularAnterior")]
    [StringLength(20)]
    [Unicode(false)]
    public string? UsuNoCelularAnterior { get; set; }

    /// <summary>
    /// Número Celular Actual del Usuario
    /// </summary>
    [Column("usuNoCelularActual")]
    [StringLength(20)]
    [Unicode(false)]
    public string UsuNoCelularActual { get; set; } = null!;

    /// <summary>
    /// Número de Boleta del Usuario Alumno/Egresado
    /// </summary>
    [Column("usuBoletaAlumnoEgresado")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuBoletaAlumnoEgresado { get; set; }

    /// <summary>
    /// Número de Boleta del Usuario Alumno de Posgrado
    /// </summary>
    [Column("usuBoletaPosgrado")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuBoletaPosgrado { get; set; }

    /// <summary>
    /// FK ID de la Carrera que pertenece o cursó
    /// </summary>
    [Column("usuIdCarrera")]
    public int? UsuIdCarrera { get; set; }

    /// <summary>
    /// Semestre que cursa o perteneció
    /// </summary>
    [Column("usuSemestre")]
    [StringLength(15)]
    [Unicode(false)]
    public string? UsuSemestre { get; set; }

    /// <summary>
    /// Año de Egreso en caso de ser Egresado
    /// </summary>
    [Column("usuAnioEgreso")]
    public int? UsuAnioEgreso { get; set; }

    /// <summary>
    /// Nombre del Archivo PDF de la Tira de Materias / Constancia o Certificado de Estudios / Boleta / SIP-10
    /// </summary>
    [Column("usuFileNameComprobanteEstudios")]
    [StringLength(200)]
    [Unicode(false)]
    public string? UsuFileNameComprobanteEstudios { get; set; }

    /// <summary>
    /// Número de Empleado del Usuario { Administrativo, Docente } / Número de Contrato del Usuario { Honorarios }
    /// </summary>
    [Column("usuNumeroEmpleadoContrato")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuNumeroEmpleadoContrato { get; set; }

    /// <summary>
    /// FK ID del Área / Departamento donde labora el Empleado
    /// </summary>
    [Column("usuIdAreaDepto")]
    public int? UsuIdAreaDepto { get; set; }

    /// <summary>
    /// Número de Extensión anterior del Área / Departamento
    /// </summary>
    [Column("usuNoExtensionAnterior")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuNoExtensionAnterior { get; set; }

    /// <summary>
    /// Número de Extensión actual del Área / Departamento
    /// </summary>
    [Column("usuNoExtensionActual")]
    [StringLength(10)]
    [Unicode(false)]
    public string? UsuNoExtensionActual { get; set; }

    /// <summary>
    /// Correo Electrónico Personal Anterior
    /// </summary>
    [Column("usuCorreoPersonalCuentaAnterior")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoPersonalCuentaAnterior { get; set; }

    /// <summary>
    /// Correo Electrónico Personal Actual
    /// </summary>
    [Column("usuCorreoPersonalCuentaActual")]
    [StringLength(100)]
    [Unicode(false)]
    public string UsuCorreoPersonalCuentaActual { get; set; } = null!;

    /// <summary>
    /// Contraseña Encriptada para ingresar a la Plataforma SACI
    /// </summary>
    [Column("usuContrasenia")]
    [StringLength(300)]
    [Unicode(false)]
    public string UsuContrasenia { get; set; } = null!;

    /// <summary>
    /// Bandera { 0 = Inicia Sesión, 1 = Pide cambiar contraseña temporal }
    /// </summary>
    [Column("usuRecuperarContrasenia")]
    public bool UsuRecuperarContrasenia { get; set; }

    /// <summary>
    /// Correo Electrónico Institucional IPN
    /// </summary>
    [Column("usuCorreoInstitucionalCuenta")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoInstitucionalCuenta { get; set; }

    /// <summary>
    /// Contraseña del Correo Electrónico Institucional IPN
    /// </summary>
    [Column("usuCorreoInstitucionalContrasenia")]
    [StringLength(100)]
    [Unicode(false)]
    public string? UsuCorreoInstitucionalContrasenia { get; set; }

    /// <summary>
    /// Fecha Hora de Alta del Usuario
    /// </summary>
    [Column("usuFechaHoraAlta", TypeName = "datetime")]
    public DateTime UsuFechaHoraAlta { get; set; }

    /// <summary>
    /// Fecha Hora en que se va actualizando el Correo Institucional y/o Contraseña de la Plataforma SACI
    /// </summary>
    [Column("usuFechaHoraActualizacion", TypeName = "datetime")]
    public DateTime UsuFechaHoraActualizacion { get; set; }

    /// <summary>
    /// Estatus { 0 = Inactivo, 1 = Activo }
    /// </summary>
    [Column("usuStatus")]
    public bool UsuStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("SolIdUsuarioNavigation")]
    public virtual ICollection<MtTbSolicitudesTicket> MtTbSolicitudesTickets { get; set; } = [];

    [ForeignKey("UsuIdAreaDepto")]
    [InverseProperty("MpTbUsuarios")]
    public virtual McCatAreasDepto? UsuIdAreaDeptoNavigation { get; set; }

    [ForeignKey("UsuIdCarrera")]
    [InverseProperty("MpTbUsuarios")]
    public virtual McCatCarrera? UsuIdCarreraNavigation { get; set; }

    [ForeignKey("UsuIdRol")]
    [InverseProperty("MpTbUsuarios")]
    public virtual McCatRole? UsuIdRolNavigation { get; set; } = null!;

    [ForeignKey("UsuIdTipoPersonal")]
    [InverseProperty("MpTbUsuarios")]
    public virtual McCatTiposPersonal? UsuIdTipoPersonalNavigation { get; set; } = null!;
}
