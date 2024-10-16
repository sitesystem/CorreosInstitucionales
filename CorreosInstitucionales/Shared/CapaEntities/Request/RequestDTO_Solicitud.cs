using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_Solicitud:MtTbSolicitudesTicket
    {
        /*******************************  DATOS ID LA SOLICITUD  *******************************/
        /// <summary>
        /// FK ID del Tipo de Solicitud (1 - A, 2 - B, 3 - C, 4 -D, 5 - E, 6 - F, 7 - G, 8 - OTRO)
        /// </summary>
        [Range(1, 10, ErrorMessage = "Selecciona un MOTIVO / INCIDENCIA de Solicitud-Ticket.")]
        public new int SolIdTipoSolicitud
        {
            get { return base.SolIdTipoSolicitud; }
            set { base.SolIdTipoSolicitud = value; }
        }

        /*******************************  DATOS DE ARCHIVOS PDF DEL USUARIO  *******************************/
        /// <summary>
        /// Nombre del Archivo PDF del CURP del Usuario
        /// </summary>
        [StringLength(200, ErrorMessage = "El Nombre del Archivo PDF del CURP adjuntado debe ser máximo de 200 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF del CURP requerido.")]
        public string? SolFileNameCurp { get; set; }

        /// <summary>
        /// Tamaño del Archivo PDF del CURP del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF del CURP adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeCurp { get; set; }

        /// <summary>
        /// Nombre del Archivo PDF de la Tira de Materias / Certificado de Calificaciones / SIP-10
        /// </summary>
        [Column("solFileNameComprobanteInscripcion")]
        [StringLength(200, ErrorMessage = "El Nombre del Archivo PDF del COMPROBANTE DE INSCRIPCIÓN / HORARIO (TIRA DE MATERIAS) del Periodo Escolar Actual adjuntado debe ser máximo de 200 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF del COMPROBANTE DE INSCRIPCIÓN / HORARIO (TIRA DE MATERIAS) del Periodo Escolar Actual requerido.")]
        public string? SolFileNameComprobanteInscripcion { get; set; }

        /// <summary>
        /// Tamaño del Archivo PDF del Comprobante de Inscripción del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF del COMPROBANTE DE INSCRIPCIÓN / HORARIO adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeComprobanteInscripcion { get; set; }

        /*******************************  DATOS DE CAMBIO DE NO. CELULAR O CORREO PERSONAL  *******************************/
        // Número de Celular Anterior del Usuario
        [StringLength(20)]
        [MinLength(14, ErrorMessage = "Verifique el No. DE CELULAR ANTERIOR que tenga al menos 10 dígitos.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE CELULAR ANTERIOR requerido.")]
        public string? SolNoCelularAnterior { get; set; }

        // Número de Celular Actual del Usuario
        [StringLength(20)]
        [MinLength(14, ErrorMessage = "Verifique el No. DE CELULAR ACTUAL que tenga al menos 10 dígitos.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo No. DE CELULAR ACTUAL requerido.")]
        public string SolNoCelularActual { get; set; } = null!;

        // Correo Electrónico Personal Anterior del Usuario
        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CORREO PERSONAL ANTERIOR requerido.")]
        [RegularExpression("^(?!.*@(?:ipn\\.mx|alumno\\.ipn\\.mx|egresado\\.ipn\\.mx)$)[\\w\\.-]+@([\\w-]+\\.)+[\\w-]{2,}$", ErrorMessage = "CORREO PERSONAL ANTERIOR inválido. (Formato: xxxxxx@xxx.xx)")]
        public string? SolCorreoPersonalCuentaAnterior { get; set; }

        // Correo Electrónico Personal Actual del Usuario
        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CORREO PERSONAL ACTUAL requerido.")]
        [RegularExpression("^(?!.*@(?:ipn\\.mx|alumno\\.ipn\\.mx|egresado\\.ipn\\.mx)$)[\\w\\.-]+@([\\w-]+\\.)+[\\w-]{2,}$", ErrorMessage = "CORREO PERSONAL ACTUAL inválido. (Formato: xxxxxx@xxx.xx)")]
        public string SolCorreoPersonalCuentaActual { get; set; } = null!;

        /*******************************  DATOS DE ARCHIVOS PDF CAPTURAS  *******************************/
        /// <summary>
        /// Nombre del Archivo PDF de la Captura del Escaneo del Antivirus
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF CAPTURA ESCANEO ANTIVIRUS requerido.")]
        public new string? SolCapturaEscaneoAntivirus
        {
            get { return base.SolCapturaEscaneoAntivirus; }
            set { base.SolCapturaEscaneoAntivirus = value; }
        }

        /// <summary>
        /// Tamaño del Archivo PDF del CURP del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF CAPTURA ESCANEO ANTIVIRUS adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeCapturaEscaneoAntivirus { get; set; }

        /// <summary>
        /// Nombre del Archivo PDF de la Captura de la Cuenta Bloqueada
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF CAPTURA CUENTA BLOQUEADA requerido.")]
        public new string? SolCapturaCuentaBloqueada
        {
            get { return base.SolCapturaCuentaBloqueada; }
            set { base.SolCapturaCuentaBloqueada = value; }
        }

        /// <summary>
        /// Tamaño del Archivo PDF del CURP del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF CAPTURA CUENTA BLOQUEADA adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeCapturaCuentaBloqueada { get; set; }

        /// <summary>
        /// Nombre del Archivo PDF de la Captura del Error
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF CAPTURA OTRO MOTIVO, INCIDENCIA O PROBLEMA requerido.")]
        public new string? SolCapturaError
        {
            get { return base.SolCapturaError; }
            set { base.SolCapturaError = value; }
        }

        /// <summary>
        /// Tamaño del Archivo PDF del CURP del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF CAPTURA OTRO MOTIVO, INCIDENCIA O PROBLEMA adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeCapturaError { get; set; }


        /*******************************  OTROS DATOS  *******************************/
        /// <summary>
        /// Observación y/o comentario del problema de la Solicitud-Ticket.
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo OBSERVACIONES / COMENTARIOS / NOTAS / DETALLE DEL MOTIVO requerido.")]
        public new string? SolObservacionesSolicitud
        {
            get { return base.SolObservacionesSolicitud; }
            set { base.SolObservacionesSolicitud = value; }
        }
    }
}
