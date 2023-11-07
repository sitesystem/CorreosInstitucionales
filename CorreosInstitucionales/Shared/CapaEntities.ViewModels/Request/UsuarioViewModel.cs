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
        /// Nombre del Usuario Solicitante
        /// </summary>
        [Column("usuNombre")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        //[RegularExpression("^[A-Z. ]$", ErrorMessage = "Formato Incorrecto.")] // NO ADMITE ACENTOS
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
        [StringLength(18)]
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
        
        public string? UsuNoCelularAnt { get; set; }

        [Column("usuNoCelularNuevo")]
        [StringLength(20)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuNoCelularNuevo { get; set; }

        /// <summary>
        /// Numero de Boleta del Uusario Solicitante
        /// </summary>
        [Column("usuBoleta")]
        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuBoleta { get; set; }

        [Column("usuSemestre")]
        [StringLength(10)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuSemestre { get; set; }

        [Column("usuAñoEgreso")]
        public int? UsuAñoEgreso { get; set; }

        /// <summary>
        /// Numero del Empleado del Usuario Solicitante
        /// </summary>
        [Column("usuNumeroEmpleado")]
        [StringLength(100)]
        
        public string? UsuNumeroEmpleado { get; set; }

        [Column("usuIdRol")]
        public int? UsuIdRol { get; set; }

        [Column("usuExtension")]
        [StringLength(20)]
        
        public string? UsuExtension { get; set; }

        /// <summary>
        /// Tipo de Personal del Usuario Solicitante
        /// </summary>
        [Column("usuIdTipoPersonal")]
        public int? UsuIdTipoPersonal { get; set; }

        [Column("usuCorreoPersonal")]
        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuCorreoPersonal { get; set; }

        /// <summary>
        /// Contraseña del Usuario Solicitante
        /// </summary>
        [Column("usuContraseña")]
        [StringLength(300)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? UsuContraseña { get; set; }

        /// <summary>
        /// Contraseña Temporal que se le proporciona al Usuario Solicitante
        /// </summary>
        [Column("usuRecuperarContraseñas")]
        public bool? UsuRecuperarContraseñas { get; set; }

        [Column("usuIdCarrera")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int? UsuIdCarrera { get; set; }

        [Column("usuCorreroInstitucional")]
        [StringLength(100)]
        
        public string? UsuCorreroInstitucional { get; set; }

        [Column("usuContraseñaInstitucional")]
        [StringLength(300)]
        
        public string? UsuContraseñaInstitucional { get; set; }

        [Column("usuFechaHoraAlta", TypeName = "datetime")]
        public DateTime? UsuFechaHoraAlta { get; set; }

        [Column("usuArchivoCapturaEscaneo")]
        [StringLength(200)]
        
        public string? UsuArchivoCapturaEscaneo { get; set; }

        [Column("usuArchivoCapturaError")]
        [StringLength(200)]
        
        public string? UsuArchivoCapturaError { get; set; }

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
