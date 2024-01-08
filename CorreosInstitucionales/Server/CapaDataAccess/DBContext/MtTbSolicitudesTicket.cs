using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MT_tbSolicitudesTickets")]
public partial class MtTbSolicitudesTicket
{
    /// <summary>
    /// ID Único de la Solicitud
    /// </summary>
    [Key]
    public int IdSolicitudTicket { get; set; }

    /// <summary>
    /// FK ID del Tipo de Solicitud (1 - A, 2 - B, 3 - C)
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
    [StringLength(150)]
    [Unicode(false)]
    public string? SolCapturaEscaneoAntivirus { get; set; }

    /// <summary>
    /// Nombre del Archivo PDF de la Captura de la Cuenta Bloqueada
    /// </summary>
    [Column("solCapturaCuentaBloqueada")]
    [StringLength(150)]
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
    /// Fecha Hora de la creación de la Solicitud del Ticket
    /// </summary>
    [Column("solFechaHoraCreacion", TypeName = "datetime")]
    public DateTime SolFechaHoraCreacion { get; set; }

    /// <summary>
    /// FK ID del Estado de la Solicitud (1 = Levantado, 2 = Pendiente, 3 = En Proceso, 4 = Atendido)
    /// </summary>
    [Column("solIdEstadoSolicitud")]
    public int SolIdEstadoSolicitud { get; set; }

    [ForeignKey("SolIdEstadoSolicitud")]
    [InverseProperty("MtTbSolicitudesTickets")]
    public virtual McCatEstadosSolicitud SolIdEstadoSolicitudNavigation { get; set; } = null!;

    [ForeignKey("SolIdTipoSolicitud")]
    [InverseProperty("MtTbSolicitudesTickets")]
    public virtual McCatTiposSolicitud SolIdTipoSolicitudNavigation { get; set; } = null!;

    [ForeignKey("SolIdUsuario")]
    [InverseProperty("MtTbSolicitudesTickets")]
    public virtual MpTbUsuario SolIdUsuarioNavigation { get; set; } = null!;
}
