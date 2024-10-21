using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

[Table("MT_tbSolicitudesTickets")]
public partial class MtTbSolicitudesTicket
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
    [Unicode(false)]
    public string SolToken { get; set; } = null!;

    /// <summary>
    /// FK ID del Tipo de Solicitud (1 - A, 2 - B, 3 - C, 4 -D, 5 - E, 6 - F, 7 - G, 8 - OTRO)
    /// </summary>
    [Column("solIdTipoSolicitud")]
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
    [Unicode(false)]
    public string? SolCapturaEscaneoAntivirus { get; set; }

    /// <summary>
    /// Nombre del Archivo PDF de la Captura de la Cuenta Bloqueada
    /// </summary>
    [Column("solCapturaCuentaBloqueada")]
    [StringLength(300)]
    [Unicode(false)]
    public string? SolCapturaCuentaBloqueada { get; set; }

    /// <summary>
    /// Nombre del Archivo PDF de la Captura del Error
    /// </summary>
    [Column("solCapturaError")]
    [StringLength(150)]
    [Unicode(false)]
    public string? SolCapturaError { get; set; }

    /// <summary>
    /// Observación y/o comentario del problema de la Solicitud-Ticket.
    /// </summary>
    [Column("solObservacionesSolicitud")]
    [StringLength(300)]
    public string? SolObservacionesSolicitud { get; set; }

    /// <summary>
    /// FK ID del Estado de la Solicitud (1 = Alta/Levantad@, 2 = Pendiente, 3 = En Proceso, 4 = Atendido, 5 = Encuesta Contestada, 6 = Cancelad@)
    /// </summary>
    [Column("solIdEstadoSolicitud")]
    public int SolIdEstadoSolicitud { get; set; }

    /// <summary>
    /// Fecha/Hora en cada cambio de Estado o Etapa de la Solicitud a PENDIENTE y a ATENDIDO/FINALIZADO o CANCELADO
    /// </summary>
    [Column("solFechaHoraActualizacion", TypeName = "datetime")]
    public DateTime? SolFechaHoraActualizacion { get; set; }

    /// <summary>
    /// Validación de Datos por el Administrador { 0 - No Validados, 1 - Validados }
    /// </summary>
    [Column("solValidacionDatos")]
    public bool SolValidacionDatos { get; set; }

    /// <summary>
    /// Número de intentos del envío de encuesta de satisfacción en la calidad del servicio: por regla inicial 1 envío y si no contesta, SACI se califica con 5 estrellas
    /// </summary>
    [Column("solEnvioEncuesta")]
    public byte? SolEnvioEncuesta { get; set; }

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
    /// Respuesta de la acción de DCyC / CRM 
    /// </summary>
    [Column("solRespuestaDCyC", TypeName = "text")]
    public string? SolRespuestaDcyC { get; set; }

    /// <summary>
    /// Fecha Hora de la creación de la Solicitud-Ticket.
    /// </summary>
    [Column("solFechaHoraCreacion", TypeName = "datetime")]
    public DateTime SolFechaHoraCreacion { get; set; }

    [ForeignKey("SolIdEstadoSolicitud")]
    [InverseProperty("MtTbSolicitudesTickets")]
    public virtual McCatEstadosSolicitud? SolIdEstadoSolicitudNavigation { get; set; } = null!;

    [ForeignKey("SolIdTipoSolicitud")]
    [InverseProperty("MtTbSolicitudesTickets")]
    public virtual McCatTiposSolicitud? SolIdTipoSolicitudNavigation { get; set; } = null!;

    [ForeignKey("SolIdUsuario")]
    [InverseProperty("MtTbSolicitudesTickets")]
    public virtual MpTbUsuario? SolIdUsuarioNavigation { get; set; } = null!;
}
