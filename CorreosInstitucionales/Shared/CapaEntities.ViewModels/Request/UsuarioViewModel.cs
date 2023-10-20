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
        public string? UsuNombre { get; set; }

        /// <summary>
        /// Primer apellido del Usuario Solicitante
        /// </summary>
        [Column("usuPrimerApellido")]
        [StringLength(150)]
        public string? UsuPrimerApellido { get; set; }

        /// <summary>
        /// Segundo Apellido del Usuario Solicitante
        /// </summary>
        [Column("usuSegundoApellido")]
        [StringLength(150)]
        public string? UsuSegundoApellido { get; set; }

        [Column("usuCURP")]
        [StringLength(18)]
        public string? UsuCurp { get; set; }

        [Column("usuFilenameCURP")]
        [StringLength(200)]
        public string? UsuFilenameCurp { get; set; }

        [Column("usuNoCelular")]
        [StringLength(10)]
        public string? UsuNoCelular { get; set; }

        /// <summary>
        /// Numero de Boleta del Uusario Solicitante
        /// </summary>
        [Column("usuBoleta")]
        [StringLength(10)]
        public string? UsuBoleta { get; set; }

        /// <summary>
        /// Numero del Empleado del Usuario Solicitante
        /// </summary>
        [Column("usuNumeroEmpleado")]
        public int? UsuNumeroEmpleado { get; set; }

        [Column("usuCorreoPersonal")]
        [StringLength(100)]
        public string? UsuCorreoPersonal { get; set; }

        /// <summary>
        /// Contraseña del Usuario Solicitante
        /// </summary>
        [Column("usuContraseñaPersonal")]
        [StringLength(300)]
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

        [Column("usuCorreroInstitucional")]
        [StringLength(100)]
        public string? UsuCorreroInstitucional { get; set; }

        [Column("usuContraseñaInstitucional")]
        [StringLength(300)]
        public string? UsuContraseñaInstitucional { get; set; }

        [Column("usuIdCarrera")]
        public int? UsuIdCarrera { get; set; }

        [Column("usuFechaHoraAlta", TypeName = "datetime")]
        public DateTime UsuFechaHoraAlta { get; set; }

        /// <summary>
        /// Activo / Inactivo
        /// </summary>
        [Column("usuStatus")]
        public bool? UsuStatus { get; set; }

        //[ForeignKey("UsuIdCarrera")]
        //[InverseProperty("MceTbUsuarios")]
        //public virtual MceCatCarrera? UsuIdCarreraNavigation { get; set; }

        //[ForeignKey("UsuIdRol")]
        //[InverseProperty("MceTbUsuarios")]
        //public virtual MceCatRole? UsuIdRolNavigation { get; set; }

        //[ForeignKey("UsuIdTipoPersonal")]
        //[InverseProperty("MceTbUsuarios")]
        //public virtual MceCatTipoPersonal? UsuIdTipoPersonalNavigation { get; set; }

    }
}
