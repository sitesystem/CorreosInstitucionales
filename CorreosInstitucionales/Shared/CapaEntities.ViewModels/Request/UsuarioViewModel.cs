using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class UsuarioViewModel
    {
        /*******************************  DATOS ID DEL USUARIO  *******************************/
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

        /*******************************  DATOS PERSONALES  *******************************/
        /// <summary>
        /// Nombre del Usuario Solicitante
        /// </summary>
        [Column("usuNombre")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        [RegularExpression("^[A-Z. ]*$", ErrorMessage = "Formato Incorrecto. Este campo rechaza acentos o caracteres especiales.")] // NO ADMITE ACENTOS
        public string UsuNombre { get; set; } = null!;

        /// <summary>
        /// Primer apellido del Usuario Solicitante
        /// </summary>
        [Column("usuPrimerApellido")]
        [StringLength(150)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string UsuPrimerApellido { get; set; } = null!;

        /// <summary>
        /// Segundo Apellido del Usuario Solicitante
        /// </summary>
        [Column("usuSegundoApellido")]
        [StringLength(150)]
        public string? UsuSegundoApellido { get; set; }

        [Column("usuCURP")]
        [StringLength(18, ErrorMessage = "El CURP introducido es mayor a 18 digitos. Verifique que el CURP tenga 18 digitos.")]
        [MinLength(18, ErrorMessage = "El CURP introducido es menor a 18 digitos. Verifique que el CURP tenga 18 digitos.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string UsuCurp { get; set; } = null!;

        [Column("usuFileNameCURP")]
        [StringLength(200)]
        public string? UsuFileNameCurp { get; set; }

        [Column("usuNoCelularAnterior")]
        [StringLength(20)]
        [MinLength(10, ErrorMessage = "Verifique su numeración que tenga al menos 10 dígitos")]
        public string? UsuNoCelularAnterior { get; set; }

        [Column("usuNoCelularNuevo")]
        [StringLength(20)]
        [MinLength(10, ErrorMessage = "El Número de Celular introducido es menor a 10 digitos. Verifique su numeración que tenga al menos 10 dígitos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string UsuNoCelularNuevo { get; set; } = null!;

        /*******************************  DATOS ACADÉMICOS  *******************************/
        /// <summary>
        /// Numero de Boleta del Uusario Solicitante
        /// </summary>
        [Column("usuBoletaAlumno")]
        [StringLength(10, ErrorMessage = "La Boleta introducida es mayor a 10 digitos. Verifique su numeración que tenga al menos 10 dígitos")]
        [MinLength(10, ErrorMessage = "La boleta introducida es menor a 10 digitos. Verifique su numeración que tenga al menos 10 dígitos")]
        [RegularExpression("^\\d{4}60\\d{4}$", ErrorMessage = "Boleta invalida.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuBoletaAlumno { get; set; }

        [Column("usuBoletaMaestria")]
        [StringLength(7, ErrorMessage = "Boleta mayor a 7 dígitios. Verifique su boleta.")]
        [MinLength(7, ErrorMessage = "La boleta es menor a 7 digitos. Verifique su boleta")]
        [RegularExpression("^B\\d{6}$", ErrorMessage = "Boleta invalida.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuBoletaMaestria { get; set; }

        [Column("usuIdCarrera")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int? UsuIdCarrera { get; set; }

        [Column("usuSemestre")]
        [StringLength(15)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuSemestre { get; set; }

        [Column("usuAñoEgreso")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int? UsuAñoEgreso { get; set; }

        [Column("usuFileNameComprobanteInscripcion")]
        [StringLength(200)]
        public string? UsuFileNameComprobanteInscripcion { get; set; }

        /*******************************  DATOS LABORALES  *******************************/
        /// <summary>
        /// Numero del Empleado del Usuario Solicitante
        /// </summary>
        [Column("usuNumeroEmpleado")]
        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuNumeroEmpleado { get; set; }

        [Column("usuIdAreaDepto")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int? UsuIdAreaDepto { get; set; }

        [Column("usuNoExtension")]
        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuNoExtension { get; set; }

        /*******************************  DATOS DE LAS CREDENCIALES DE LA CUENTA EN LA APP  *******************************/
        [Column("usuCorreoPersonalCuentaAnterior")]
        [StringLength(100)]
        public string? UsuCorreoPersonalCuentaAnterior { get; set; }

        [Column("usuCorreoPersonalCuentaNueva")]
        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string UsuCorreoPersonalCuentaNueva { get; set; } = null!;

        /// <summary>
        /// Contraseña del Usuario Solicitante
        /// </summary>
        [Column("usuContraseña")]
        [StringLength(300)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string UsuContraseña { get; set; } = null!;

        /// <summary>
        /// Contraseña Temporal que se le proporciona al Usuario Solicitante
        /// </summary>
        [Column("usuRecuperarContraseña")]
        public bool? UsuRecuperarContraseña { get; set; }

        /*******************************  DATOS DEL CORREO INSTITUCIONAL  *******************************/
        [Column("usuCorreoInstitucionalCuenta")]
        [StringLength(100)]
        public string? UsuCorreoInstitucionalCuenta { get; set; }

        [Column("usuCorreoInstitucionalContraseña")]
        [StringLength(100)]
        public string? UsuCorreoInstitucionalContraseña { get; set; }

        /*******************************  OTROS DATOS  *******************************/
        [Column("usuFechaHoraAlta", TypeName = "datetime")]
        public DateTime UsuFechaHoraAlta { get; set; }

        /// <summary>
        /// Activo / Inactivo
        /// </summary>
        [Required]
        [Column("usuStatus")]
        public bool? UsuStatus { get; set; }

        /*******************************  DATOS FK NAVIGATION  *******************************/

        //[InverseProperty("SolIdUsuarioNavigation")]
        //public virtual ICollection<SolicitudViewModel> MceTbSolicitudTickets { get; set; } = new List<SolicitudViewModel>();

        [ForeignKey("UsuIdAreaDepto")]
        [InverseProperty("MceTbUsuarios")]
        public virtual AreaDeptoViewModel? UsuIdAreaDeptoNavigation { get; set; }

        [ForeignKey("UsuIdCarrera")]
        [InverseProperty("MceTbUsuarios")]
        public virtual CarreraViewModel? UsuIdCarreraNavigation { get; set; }

        [ForeignKey("UsuIdRol")]
        [InverseProperty("MceTbUsuarios")]
        public virtual RolViewModel? UsuIdRolNavigation { get; set; } = null!;

        [ForeignKey("UsuIdTipoPersonal")]
        [InverseProperty("MceTbUsuarios")]
        public virtual TipoPersonalViewModel? UsuIdTipoPersonalNavigation { get; set; } = null!;
    }
}
