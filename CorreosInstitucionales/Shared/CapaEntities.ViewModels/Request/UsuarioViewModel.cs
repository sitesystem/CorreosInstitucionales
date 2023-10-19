using System;
using System.Collections.Generic;
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
        public int IdUsuarioSolicitante { get; set; }

        /// <summary>
        /// Nombre del Usuario Solicitante
        /// </summary>
        public string? UsuNombre { get; set; }

        /// <summary>
        /// Primer apellido del Usuario Solicitante
        /// </summary>
        public string? UsuPrimerApellido { get; set; }

        /// <summary>
        /// Segundo Apellido del Usuario Solicitante
        /// </summary>
        public string? UsuSegundoApellido { get; set; }

        /// <summary>
        /// Numero de Boleta del Uusario Solicitante
        /// </summary>
        public string? UsuBoleta { get; set; }

        /// <summary>
        /// Numero del Empleado del Usuario Solicitante
        /// </summary>
        public int? UsuNumeroEmpleado { get; set; }

        /// <summary>
        /// Contraseña del Usuario Solicitante
        /// </summary>
        public string? UsuContraseña { get; set; }

        /// <summary>
        /// Contraseña Temporal que se le proporciona al Usuario Solicitante
        /// </summary>
        public bool? UsuRecuperarContraseñas { get; set; }

        /// <summary>
        /// Tipo de Personal del Usuario Solicitante
        /// </summary>
        public int? UsuIdTipoPersonal { get; set; }

        /// <summary>
        /// Activo / Inactivo
        /// </summary>
        public bool? UsuStatus { get; set; }

        public string? UsuCorreoPersonal { get; set; }

        public int? UsuIdRol { get; set; }

        public string? UsuCorreroInstitucional { get; set; }

        public string? UsuContraseñaInstitucional { get; set; }

        public int? UsuIdCarrera { get; set; }

        //public virtual MceCatTipoPersonal? UsuIdTipoPersonalNavigation { get; set; }
    }
}
