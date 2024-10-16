using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaEntities.Request;

public class RequestDTO_Usuario: MpTbUsuario
{
    /*******************************  DATOS PERSONALES  *******************************/
    /// <summary>
    /// Nombre del Usuario Solicitante o Administrador
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE(S) requerido.")]
    [RegularExpression("^[a-zA-Z. ]*$", ErrorMessage = "Formato Incorrecto (No se permite acentos o caracteres especiales).")] // NO ADMITE ACENTOS
    public new string UsuNombre
    {
        get { return base.UsuNombre; }
        set { base.UsuNombre = value; }
    }


    /// <summary>
    /// Primer Apellido del Usuario
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo PRIMER APELLIDO requerido.")]
    [RegularExpression("^[a-zA-Z. ]*$", ErrorMessage = "Formato Incorrecto (No se permite acentos o caracteres especiales).")] // NO ADMITE ACENTOS
    public new string UsuPrimerApellido
    {
        get { return base.UsuPrimerApellido; }
        set { base.UsuPrimerApellido = value; }
    }

    /// <summary>
    /// Segundo Apellido del Usuario
    /// </summary>
    [RegularExpression("^[a-zA-Z. ]*$", ErrorMessage = "Formato Incorrecto (No se permite acentos o caracteres especiales).")] // NO ADMITE ACENTOS
    public new string? UsuSegundoApellido
    {
        get { return base.UsuSegundoApellido; }
        set { base.UsuSegundoApellido = value; }
    }


    /// <summary>
    /// CURP del Usuario
    /// </summary>
    [StringLength(18, ErrorMessage = "El CURP introducido debe ser máximo de 18 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CURP requerido.")]
    [MinLength(18, ErrorMessage = "El CURP introducido debe ser mínimo de 18 caracteres.")]
    public new string UsuCurp
    {
        get { return base.UsuCurp; }
        set { base.UsuCurp = value; }
    }


    /// <summary>
    /// Nombre del Archivo PDF del CURP del Usuario
    /// </summary>
    [StringLength(200, ErrorMessage = "El Nombre del Archivo PDF del CURP adjuntado debe ser máximo de 200 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF del CURP requerido.")]
    public new string? UsuFileNameCurp
    {
        get { return base.UsuFileNameCurp; }
        set { base.UsuFileNameCurp = value; }
    }

    /// <summary>
    /// Tamaño del Archivo PDF del CURP del Usuario
    /// </summary>
    [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF del CURP adjuntado debe ser máximo de 2 MB.")]
    public long? UsuFileSizeCurp { get; set; }

    /// <summary>
    /// Número Celular Anterior del Usuario
    /// </summary>
    [MinLength(14, ErrorMessage = "Verifique el No. DE CELULAR ANTERIOR que tenga al menos 10 dígitos.")]
    public new string? UsuNoCelularAnterior
    {
        get { return base.UsuNoCelularAnterior; }
        set { base.UsuNoCelularAnterior = value; }
    }

    /// <summary>
    /// Número Celular Actual/Nuevo del Usuario
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE CELULAR ACTUAL requerido.")]
    [MinLength(14, ErrorMessage = "El No. DE CELULAR ACTUAL debe ser mínimo de 10 dígitos.")]
    public new string UsuNoCelularNuevo
    {
        get { return base.UsuNoCelularNuevo; }
        set { base.UsuNoCelularNuevo = value; }
    }

    /*******************************  DATOS ACADÉMICOS  *******************************/
    /// <summary>
    /// Número de Boleta del Usuario Alumno/Egresado
    /// </summary>
    [StringLength(10, ErrorMessage = "La BOLETA introducida debe ser máximo de 10 dígitos.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo BOLETA requerido.")]
    [MinLength(10, ErrorMessage = "La BOLETA introducida debe ser mínimo de 10 dígitos.")]
    // [RegularExpression("^\\d{4}60\\d{4}$", ErrorMessage = "BOLETA Inválida (Formato: xxxx60xxxx).")]
    public new string? UsuBoletaAlumno
    {
        get { return base.UsuBoletaAlumno; }
        set { base.UsuBoletaAlumno = value; }
    }

    /// <summary>
    /// Número de Boleta del Usuario Alumno de Maestría
    /// </summary>
    [StringLength(7, ErrorMessage = "La BOLETA introducida debe ser máximo de 7 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo BOLETA requerido.")]
    [MinLength(7, ErrorMessage = "La BOLETA introducida debe ser mínimo de 7 caracteres.")]
    [RegularExpression("^B\\d{6}$", ErrorMessage = "BOLETA Inválida (Formato: Bxxxxxx).")]
    public new string? UsuBoletaMaestria
    {
        get { return base.UsuBoletaMaestria; }
        set { base.UsuBoletaMaestria = value; }
    }

    /// <summary>
    /// FK ID de la Carrera que pertenece o cursó
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CARRERA / LICENCIATURA requerido.")]
    public new int? UsuIdCarrera
    {
        get { return base.UsuIdCarrera; }
        set { base.UsuIdCarrera = value; }
    }

    /// <summary>
    /// Semestre que cursa
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo SEMESTRE requerido.")]
    public new string? UsuSemestre
    {
        get { return base.UsuSemestre; }
        set { base.UsuSemestre = value; }
    }

    /// <summary>
    /// Año de Egreso en caso de ser Alumno Egresado
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo AÑO DE EGRESO requerido.")]
    public new int? UsuAnioEgreso
    {
        get { return base.UsuAnioEgreso; }
        set { base.UsuAnioEgreso = value; }
    }

    /// <summary>
    /// Nombre del Archivo PDF de la Tira de Materias / Certificado de Calificaciones / SIP-10
    /// </summary>
    [StringLength(200, ErrorMessage = "El Nombre del Archivo PDF del COMPROBANTE DE INSCRIPCIÓN/ESTUDIOS/HORARIO adjuntado debe ser máximo de 200 caracteres.")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF del COMPROBANTE DE INSCRIPCIÓN/ESTUDIOS/HORARIO (Tira de Materias) del Periodo Escolar Actual requerido.")]
    public new string? UsuFileNameComprobanteInscripcion
    {
        get { return base.UsuFileNameComprobanteInscripcion; }
        set { base.UsuFileNameComprobanteInscripcion = value; }
    }

    /// <summary>
    /// Tamaño del Archivo PDF del COMPROBANTE DE INSCRIPCIÓN/ESTUDIOS del Usuario
    /// </summary>
    [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF del COMPROBANTE DE INSCRIPCIÓN/ESTUDIOS/HORARIO adjuntado debe ser máximo de 2 MB.")]
    public long? UsuFileSizeComprobanteInscripcion { get; set; }

    /*******************************  DATOS LABORALES  *******************************/
    /// <summary>
    /// Número de Empleado del Usuario { Administrativo, Docente }
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE EMPLEADO requerido.")]
    public new string? UsuNumeroEmpleado
    {
        get { return base.UsuNumeroEmpleado; }
        set { base.UsuNumeroEmpleado = value; }
    }

    /// <summary>
    /// FK ID del Área / Departamento domde labora el Empleado
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo ÁREA / DEPARTAMENTO requerido.")]
    public new int? UsuIdAreaDepto
    {
        get { return base.UsuIdAreaDepto; }
        set { base.UsuIdAreaDepto = value; }
    }

    /// <summary>
    /// Número de Extensión del Área / Departamento
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE EXTENSIÓN requerido.")]
    public new string? UsuNoExtension
    {
        get { return base.UsuNoExtension; }
        set { base.UsuNoExtension = value; }
    }

    /*******************************  DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP  *******************************/

    /// <summary>
    /// Correo Personal Actual / Nuevo
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CORREO ELECTRÓNICO PERSONAL requerido.")]
    //[RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "CORREO inválido. (Formato: xxxxxx@xxx.xx)")]
    [RegularExpression("^(?!.*@(?:ipn\\.mx|alumno\\.ipn\\.mx|egresado\\.ipn\\.mx)$)[\\w\\.-]+@([\\w-]+\\.)+[\\w-]{2,}$", ErrorMessage = "CORREO PERSONAL inválido. (Formato correcto: xxxxxx@xxx.xx)")]
    public new string UsuCorreoPersonalCuentaNueva
    {
        get { return base.UsuCorreoPersonalCuentaNueva; }
        set { base.UsuCorreoPersonalCuentaNueva = value; }
    }


    /// <summary>
    /// Contraseña Encriptada en la Plataforma SACI
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CONTRASEÑA requerido.")]
    public new string UsuContrasenia
    {
        get { return base.UsuContrasenia; }
        set { base.UsuContrasenia = value; }
    }

    /*******************************  DATOS DEL CORREO INSTITUCIONAL  *******************************/
    /// <summary>
    /// Correo Electrónico Institucional IPN
    /// </summary>
    [RegularExpression("^[\\w-\\.]+@ipn\\.mx$|^[\\w-\\.]+@alumno\\.ipn\\.mx$|^[\\w-\\.]+@egresado\\.ipn\\.mx$", ErrorMessage = "CORREO inválido. (Formato: xxxxxx@ipn.mx ó @alumno.ipn.mx ó @egresado.ipn.mx)")]
    public new string? UsuCorreoInstitucionalCuenta
    {
        get { return base.UsuCorreoInstitucionalCuenta; }
        set { base.UsuCorreoInstitucionalCuenta = value; }
    }

}
