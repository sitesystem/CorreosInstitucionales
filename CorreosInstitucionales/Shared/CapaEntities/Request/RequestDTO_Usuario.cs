using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CorreosInstitucionales.Shared.CapaEntities.Request;

public class RequestDTO_Usuario
{
    /*******************************  DATOS ID DEL USUARIO  *******************************/
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

    /*******************************  DATOS PERSONALES  *******************************/
    /// <summary>
    /// Nombre del Usuario Solicitante o Administrador
    /// </summary>
    [Column("usuNombre")]
    [StringLength(200)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE(S) requerido.")]
    [RegularExpression("^[a-zA-Z. ]*$", ErrorMessage = "Formato Incorrecto (No se permite acentos o caracteres especiales).")] // NO ADMITE ACENTOS
    public string UsuNombre { get; set; } = null!;

    /// <summary>
    /// Primer Apellido del Usuario
    /// </summary>
    [Column("usuPrimerApellido")]
    [StringLength(150)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo PRIMER APELLIDO requerido.")]
    [RegularExpression("^[a-zA-Z. ]*$", ErrorMessage = "Formato Incorrecto (No se permite acentos o caracteres especiales).")] // NO ADMITE ACENTOS
    public string UsuPrimerApellido { get; set; } = null!;

    /// <summary>
    /// Segundo Apellido del Usuario
    /// </summary>
    [Column("usuSegundoApellido")]
    [StringLength(150)]
    [RegularExpression("^[a-zA-Z. ]*$", ErrorMessage = "Formato Incorrecto (No se permite acentos o caracteres especiales).")] // NO ADMITE ACENTOS
    public string? UsuSegundoApellido { get; set; }

    /// <summary>
    /// CURP del Usuario
    /// </summary>
    [Column("usuCURP")]
    [StringLength(18, ErrorMessage = "El CURP introducido debe ser máximo de 18 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CURP requerido.")]
    [MinLength(18, ErrorMessage = "El CURP introducido debe ser mínimo de 18 caracteres.")]
    public string UsuCurp { get; set; } = null!;


    /// <summary>
    /// Nombre del Archivo PDF del CURP del Usuario
    /// </summary>
    [Column("usuFileNameCURP")]
    [StringLength(200, ErrorMessage = "El Nombre del Archivo PDF del CURP adjuntado debe ser máximo de 200 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF del CURP requerido.")]
    public string? UsuFileNameCurp { get; set; }

    /// <summary>
    /// Tamaño del Archivo PDF del CURP del Usuario
    /// </summary>
    [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF del CURP adjuntado debe ser máximo de 2 MB.")]
    public long? UsuFileSizeCurp { get; set; }

    /// <summary>
    /// Número Celular Anterior del Usuario
    /// </summary>
    [Column("usuNoCelularAnterior")]
    [StringLength(20)]
    [MinLength(14, ErrorMessage = "Verifique el No. DE CELULAR ANTERIOR que tenga al menos 10 dígitos.")]
    public string? UsuNoCelularAnterior { get; set; }

    /// <summary>
    /// Número Celular Actual/Nuevo del Usuario
    /// </summary>
    [Column("usuNoCelularNuevo")]
    [StringLength(20)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE CELULAR ACTUAL requerido.")]
    [MinLength(14, ErrorMessage = "El No. DE CELULAR ACTUAL debe ser mínimo de 10 dígitos.")]
    public string UsuNoCelularNuevo { get; set; } = null!;

    /*******************************  DATOS ACADÉMICOS  *******************************/
    /// <summary>
    /// Número de Boleta del Usuario Alumno/Egresado
    /// </summary>
    [Column("usuBoletaAlumno")]
    [StringLength(10, ErrorMessage = "La BOLETA introducida debe ser máximo de 10 dígitos.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo BOLETA requerido.")]
    [MinLength(10, ErrorMessage = "La BOLETA introducida debe ser mínimo de 10 dígitos.")]
    [RegularExpression("^\\d{4}60\\d{4}$", ErrorMessage = "BOLETA Inválida (Formato: xxxx60xxxx).")]
    public string? UsuBoletaAlumno { get; set; }

    /// <summary>
    /// Número de Boleta del Usuario Alumno de Maestría
    /// </summary>
    [Column("usuBoletaMaestria")]
    [StringLength(7, ErrorMessage = "La BOLETA introducida debe ser máximo de 7 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo BOLETA requerido.")]
    [MinLength(7, ErrorMessage = "La BOLETA introducida debe ser mínimo de 7 caracteres.")]
    [RegularExpression("^B\\d{6}$", ErrorMessage = "BOLETA Inválida (Formato: Bxxxxxx).")]
    public string? UsuBoletaMaestria { get; set; }

    /// <summary>
    /// FK ID de la Carrera que pertenece o cursó
    /// </summary>
    [Column("usuIdCarrera")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CARRERA / LICENCIATURA requerido.")]
    public int? UsuIdCarrera { get; set; }

    /// <summary>
    /// Semestre que cursa
    /// </summary>
    [Column("usuSemestre")]
    [StringLength(15)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo SEMESTRE requerido.")]
    public string? UsuSemestre { get; set; }

    /// <summary>
    /// Año de Egreso en caso de ser Alumno Egresado
    /// </summary>
    [Column("usuAñoEgreso")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo AÑO DE EGRESO requerido.")]
    public int UsuAñoEgreso { get; set; }

    /// <summary>
    /// Nombre del Archivo PDF de la Tira de Materias / Certificado de Calificaciones / SIP
    /// </summary>
    [Column("usuFileNameComprobanteInscripcion")]
    [StringLength(200, ErrorMessage = "El Nombre del Archivo PDF del Comprobante de Inscripción adjuntado debe ser máximo de 200 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF del Comprobante de Inscripción requerido.")]
    public string? UsuFileNameComprobanteInscripcion { get; set; }

    /// <summary>
    /// Tamaño del Archivo PDF del Comprobante de Inscripción del Usuario
    /// </summary>
    [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF del Comprobante de Inscripción adjuntado debe ser máximo de 2 MB.")]
    public long? UsuFileSizeComprobanteInscripcion { get; set; }

    /*******************************  DATOS LABORALES  *******************************/
    /// <summary>
    /// Número de Empleado del Usuario { Administrativo, Docente }
    /// </summary>
    [Column("usuNumeroEmpleado")]
    [StringLength(100)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE EMPLEADO requerido.")]
    public string? UsuNumeroEmpleado { get; set; }

    /// <summary>
    /// FK ID del Área / Departamento domde labora el Empleado
    /// </summary>
    [Column("usuIdAreaDepto")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo ÁREA / DEPARTAMENTO requerido.")]
    public int? UsuIdAreaDepto { get; set; }

    /// <summary>
    /// Número de Extensión del Área / Departamento
    /// </summary>
    [Column("usuNoExtension")]
    [StringLength(10)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE EXTENSIÓN requerido.")]
    public string? UsuNoExtension { get; set; }

    /*******************************  DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP  *******************************/
    /// <summary>
    /// Correo Personal Anterior
    /// </summary>
    [Column("usuCorreoPersonalCuentaAnterior")]
    [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Correo inválido. (Formato: xxxxxx@xxx.xx)")]
    [StringLength(100)]
    public string? UsuCorreoPersonalCuentaAnterior { get; set; }

    /// <summary>
    /// Correo Personal Actual / Nuevo
    /// </summary>
    [Column("usuCorreoPersonalCuentaNueva")]
    [StringLength(100)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CORREO ELECTRÓNICO PERSONAL requerido.")]
    [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "CORREO inválido. (Formato: xxxxxx@xxx.xx)")]
    public string UsuCorreoPersonalCuentaNueva { get; set; } = null!;

    /// <summary>
    /// Contraseña Encriptada en la Plataforma SACI
    /// </summary>
    [Column("usuContraseña")]
    [StringLength(300)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CONTRASEÑA requerido.")]
    public string UsuContraseña { get; set; } = null!;

    /// <summary>
    /// Bandera { 0 = Inicia Sesión, 1 = Pide cambiar contraseña temporal }
    /// </summary>
    [Column("usuRecuperarContraseña")]
    public bool? UsuRecuperarContraseña { get; set; }

    /*******************************  DATOS DEL CORREO INSTITUCIONAL  *******************************/
    /// <summary>
    /// Correo Electrónico Institucional IPN
    /// </summary>
    [Column("usuCorreoInstitucionalCuenta")]
    [RegularExpression("^[\\w-\\.]+@ipn\\.mx$|^[\\w-\\.]+@alumno\\.ipn\\.mx$|^[\\w-\\.]+@egresado\\.ipn\\.mx$", ErrorMessage = "CORREO inválido. (Formato: xxxxxx@ipn.mx ó @alumno.ipn.mx ó @egresado.ipn.mx)")]
    [StringLength(100)]
    public string? UsuCorreoInstitucionalCuenta { get; set; }

    /// <summary>
    /// Contraseña del Correo Electrónico Institucional IPN
    /// </summary>
    [Column("usuCorreoInstitucionalContraseña")]
    [StringLength(100)]
    public string? UsuCorreoInstitucionalContraseña { get; set; }

    /*******************************  OTROS DATOS  *******************************/
    /// <summary>
    /// Fecha Hora del Alta del Usuario
    /// </summary>
    [Column("usuFechaHoraAlta", TypeName = "datetime")]
    public DateTime? UsuFechaHoraAlta { get; set; }

    /// <summary>
    /// Estatus { 0 = Inactivo, 1 = Activo }
    /// </summary>
    [Column("usuStatus")]
    [Required]
    public bool? UsuStatus { get; set; }

    /*******************************  DATOS FK NAVIGATION  *******************************/
    //[JsonIgnore]
    //[InverseProperty("SolIdUsuarioNavigation")]
    //public virtual ICollection<RequestDTO_Solicitud> MtTbSolicitudesTickets { get; set; } = new List<RequestDTO_Solicitud>();

    [ForeignKey("UsuIdAreaDepto")]
    [InverseProperty("MceTbUsuarios")]
    public virtual RequestViewModel_AreaDepto? UsuIdAreaDeptoNavigation { get; set; }

    [ForeignKey("UsuIdCarrera")]
    [InverseProperty("MceTbUsuarios")]
    public virtual RequestViewModel_Carrera? UsuIdCarreraNavigation { get; set; }

    [ForeignKey("UsuIdRol")]
    [InverseProperty("MceTbUsuarios")]
    public virtual RequestViewModel_Rol? UsuIdRolNavigation { get; set; } = null!;

    [ForeignKey("UsuIdTipoPersonal")]
    [InverseProperty("MceTbUsuarios")]
    public virtual RequestViewModel_TipoPersonal? UsuIdTipoPersonalNavigation { get; set; } = null!;
}
