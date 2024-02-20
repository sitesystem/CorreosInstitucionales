using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_Solicitud
    {
        /// <summary>
        /// PK ID Único de la Solicitud
        /// </summary>
        [Key]
        public int IdSolicitudTicket { get; set; }

        /// <summary>
        /// Magic Link con Token para Encuestra de Calidad
        /// </summary>
        [Column("solToken")]
        [StringLength(300)]
        public string SolToken { get; set; } = null!;

        /// <summary>
        /// FK ID del Tipo de Solicitud (1 - A, 2 - B, 3 - C, 4 -D, 5 - E, 6 - F, 7 - G, 8 - OTRO)
        /// </summary>
        [Column("solIdTipoSolicitud")]
        [Range(1, 10, ErrorMessage = "Selecciona un MOTIVO / INCIDENCIA de Solicitud-Ticket.")]
        public int SolIdTipoSolicitud { get; set; }

        /// <summary>
        /// FK ID del Usuario Solicitante
        /// </summary>
        [Column("solIdUsuario")]
        public int SolIdUsuario { get; set; }

        /// <summary>
        /// Nombre del Archivo PDF de la Captura del Escaneo del Antivirus
        /// </summary>
        [Column("solCapturaEscaneoAntivirus")]
        [StringLength(300)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF CAPTURA ESCANEO ANTIVIRUS requerido.")]
        public string? SolCapturaEscaneoAntivirus { get; set; }

        /// <summary>
        /// Tamaño del Archivo PDF del CURP del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF CAPTURA ESCANEO ANTIVIRUS adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeCapturaEscaneoAntivirus { get; set; }

        /// <summary>
        /// Nombre del Archivo PDF de la Captura de la Cuenta Bloqueada
        /// </summary>
        [Column("solCapturaCuentaBloqueada")]
        [StringLength(300)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF CAPTURA CUENTA BLOQUEADA requerido.")]
        public string? SolCapturaCuentaBloqueada { get; set; }

        /// <summary>
        /// Tamaño del Archivo PDF del CURP del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF CAPTURA CUENTA BLOQUEADA adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeCapturaCuentaBloqueada { get; set; }

        /// <summary>
        /// Nombre del Archivo PDF de la Captura del Error
        /// </summary>
        [Column("solCapturaError")]
        [StringLength(150)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Archivo PDF CAPTURA ERROR requerido.")]
        public string? SolCapturaError { get; set; }

        /// <summary>
        /// Tamaño del Archivo PDF del CURP del Usuario
        /// </summary>
        [Range(1, 2000000, ErrorMessage = "El Tamaño del Archivo PDF CAPTURA ERROR adjuntado debe ser máximo de 2 MB.")]
        public long? SolFileSizeCapturaError { get; set; }

        /// <summary>
        /// Observación y/o comentario del problema de la Solicitud-Ticket.
        /// </summary>
        [Column("solObservacionesSolicitud")]
        [StringLength(300)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo OBSERVACIONES requerido.")]
        public string? SolObservacionesSolicitud { get; set; }

        /// <summary>
        /// FK ID del Estado de la Solicitud (1 = Alta/Levantad@, 2 = Pendiente, 3 = En Proceso, 4 = Atendido)
        /// </summary>
        [Column("solIdEstadoSolicitud")]
        public int SolIdEstadoSolicitud { get; set; }

        /// <summary>
        /// Validación de Datos por el Administrador { 0 - No Validados, 1 - Validados }
        /// </summary>
        [Column("solValidacionDatos")]
        public bool SolValidacionDatos { get; set; }

        /// <summary>
        /// Calificación de la Encuesta de Calidad con emojis caritas o estrellas.
        /// </summary>
        [Column("solEncuestaCalidadCalificacion")]
        public int? SolEncuestaCalidadCalificacion { get; set; }

        /// <summary>
        /// Comentarios, observaciones o notas por la atención de la Solicitud-Ticket.
        /// </summary>
        [Column("solEncuestaCalidadComentarios")]
        [StringLength(300)]
        public string? SolEncuestaCalidadComentarios { get; set; }

        /// <summary>
        /// Fecha Hora de la respuesta a la Encuesta de Calidad de la Solicitud-Ticket.
        /// </summary>
        [Column("solFechaHoraEncuesta", TypeName = "datetime")]
        public DateTime? SolFechaHoraEncuesta { get; set; }

        /// <summary>
        /// Fecha Hora de la creación de la Solicitud-Ticket.
        /// </summary>
        [Column("solFechaHoraCreacion", TypeName = "datetime")]
        public DateTime SolFechaHoraCreacion { get; set; }

        [ForeignKey("SolIdEstadoSolicitud")]
        [InverseProperty("MceTbSolicitudTickets")]
        public virtual RequestViewModel_EstadoSolicitud? SolIdEstadoSolicitudNavigation { get; set; } = null!;

        [ForeignKey("SolIdTipoSolicitud")]
        [InverseProperty("MceTbSolicitudTickets")]
        public virtual RequestViewModel_TipoSolicitud? SolIdTipoSolicitudNavigation { get; set; } = null!;

        [ForeignKey("SolIdUsuario")]
        [InverseProperty("MceTbSolicitudTickets")]
        public virtual RequestDTO_Usuario? SolIdUsuarioNavigation { get; set; } = null!;
    }
}
