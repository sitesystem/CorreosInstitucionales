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

        /// <summary>
        /// Descripcion del Usuario Solicitante
        /// </summary>
        [Key]
        public int IdUsuarioSolicitante { get; set; }

        /// <summary>
        /// Tipo de Personal del Usuario Solicitante
        /// </summary>
        [Column("usuIdTipoPersonal")]
        public int? UsuIdTipoPersonal { get; set; }

        /// <summary>
        /// Nombre del Usuario Solicitante
        /// </summary>
        [Column("usuNombre")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        [RegularExpression("^[A-Z. ]*$", ErrorMessage = "Formato Incorrecto. Este campo rechaza acentos o caracteres especiales")] // NO ADMITE ACENTOS
        public string? UsuNombre { get; set; }

        /// <summary>
        /// Primer apellido del Usuario Solicitante
        /// </summary>
        [Column("usuPrimerApellido")]
        [StringLength(150)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuPrimerApellido { get; set; }

        /// <summary>
        /// Segundo Apellido del Usuario Solicitante
        /// </summary>
        [Column("usuSegundoApellido")]
        [StringLength(150)]
        public string? UsuSegundoApellido { get; set; }

        [Column("usuCURP")]
        [StringLength(18, ErrorMessage = "El CUPR introducido es mayor a 18 digitos. Verifique que el CURP tenga 18 digitos")]
        [MinLength(18, ErrorMessage = "El CUPR introducido es menor a 18 digitos. Verifique que el CURP tenga 18 digitos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuCurp { get; set; }

        [Column("usuFilenameCURP")]
        [StringLength(200)]
        public string? UsuFilenameCurp { get; set; }

        [Column("usuArchivoCompInscripcion")]
        [StringLength(200)]
        public string? UsuArchivoCompInscripcion { get; set; }

        [Column("usuNoCelularAnt")]
        [StringLength(20)]
        //[MinLength(10, ErrorMessage = "Verifique su numeración que tenga al menos 10 dígitos")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuNoCelularAnterior { get; set; }

        [Column("usuNoCelularNuevo")]
        [StringLength(20)]
        [MinLength(10, ErrorMessage = "El CUPR introducido es menor a 10 digitos. Verifique su numeración que tenga al menos 10 dígitos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuNoCelularNuevo { get; set; }

        [Column("usuIdCarrera")]
        public int? UsuIdCarrera { get; set; }

        /// <summary>
        /// Numero de Boleta del Uusario Solicitante
        /// </summary>
        [Column("usuBoletaAlumno")]
        [StringLength(10, ErrorMessage = "La boleta introducida es menor a 10 digitos. Verifique su numeración que tenga 10 dígitos")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuBoletaAlumno { get; set; }

        [Column("usuBoletaMaestria")]
        [StringLength(7)]        
        public string? UsuBoletaMaestria { get; set; }

        [Column("usuSemestre")]
        [StringLength(15)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuSemestre { get; set; }

        [Column("usuAñoEgreso")]
        [Required]
        public int? UsuAñoEgreso { get; set; }

        /// <summary>
        /// Numero del Empleado del Usuario Solicitante
        /// </summary>
        [Column("usuNumeroEmpleado")]
        [StringLength(100)]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuNumeroEmpleado { get; set; }


        [Column("usuIdAreaDepto")]
        [StringLength(100)]        
        public string? UsuIdAreaDepto { get; set; }

        [Column("usuExtension")]
        [StringLength(20)]       
        public string? UsuExtension { get; set; }

        [Column("usuIdRol")]
        public int? UsuIdRol { get; set; }

        [Column("usuCorreoPersonalAnterior")]
        [StringLength(100)]        
        public string? UsuCorreoPersonalAnterior { get; set; }

        [Column("usuCorreoPersonalNuevo")]
        [StringLength(100)]       
        public string? UsuCorreoPersonalNuevo { get; set; }

        /// <summary>
        /// Contraseña del Usuario Solicitante
        /// </summary>
        [Column("usuContraseña")]
        [StringLength(300)]
        public string? UsuContraseña { get; set; }

        /// <summary>
        /// Contraseña Temporal que se le proporciona al Usuario Solicitante
        /// </summary>
        [Column("usuRecuperarContraseñas")]
        public bool? UsuRecuperarContraseñas { get; set; }

        [Column("usuCorreroInstitucional")]
        [StringLength(100)]
        public string? UsuCorreroInstitucional { get; set; }

        [Column("usuContraseñaInstitucional")]
        [StringLength(300)]
        public string? UsuContraseñaInstitucional { get; set; }

        [Column("usuFechaHoraAlta")]
        [StringLength(100)]
        public string? UsuFechaHoraAlta { get; set; }

        /// <summary>
        /// Activo / Inactivo
        /// </summary>
        [Column("usuStatus")]
        public bool? UsuStatus { get; set; }
                      
        [ForeignKey("UsuIdCarrera")]
        [InverseProperty("MceTbUsuarios")]
        public virtual CarreraViewModel? UsuIdCarreraNavigation { get; set; }

        [ForeignKey("UsuIdRol")]
        [InverseProperty("MceTbUsuarios")]
        public virtual RolViewModel? UsuIdRolNavigation { get; set; }

        [ForeignKey("UsuIdTipoPersonal")]
        [InverseProperty("MceTbUsuarios")]
        public virtual TipoPersonalViewModel? UsuIdTipoPersonalNavigation { get; set; }
    }
}


