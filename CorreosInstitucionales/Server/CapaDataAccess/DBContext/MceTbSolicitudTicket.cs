using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_tbSolicitudTicket")]
public partial class MceTbSolicitudTicket
{
    [Key]
    public int IdSolicitudTicket { get; set; }

    [Column("solIdTipoSolicitud")]
    public int SolIdTipoSolicitud { get; set; }

    [Column("solIdUsuario")]
    public int SolIdUsuario { get; set; }

    [Column("solCapturaEscaneoAntivirus")]
    [StringLength(150)]
    [Unicode(false)]
    public string? SolCapturaEscaneoAntivirus { get; set; }

    [Column("solCapturaCuentaBloqueada")]
    [StringLength(150)]
    [Unicode(false)]
    public string? SolCapturaCuentaBloqueada { get; set; }

    [Column("solCapturaError")]
    [StringLength(150)]
    [Unicode(false)]
    public string? SolCapturaError { get; set; }

    [Column("solFechaHoraCreacion", TypeName = "datetime")]
    public DateTime SolFechaHoraCreacion { get; set; }

    [Column("solIdEstadoSolicitud")]
    public int SolIdEstadoSolicitud { get; set; }

    [ForeignKey("SolIdEstadoSolicitud")]
    [InverseProperty("MceTbSolicitudTickets")]
    public virtual MceCatEstadosSolicitud? SolIdEstadoSolicitudNavigation { get; set; } = null!;

    [ForeignKey("SolIdTipoSolicitud")]
    [InverseProperty("MceTbSolicitudTickets")]
    public virtual MceCatTipoSolicitud? SolIdTipoSolicitudNavigation { get; set; } = null!;

    [ForeignKey("SolIdUsuario")]
    [InverseProperty("MceTbSolicitudTickets")]
    public virtual MceTbUsuario? SolIdUsuarioNavigation { get; set; } = null!;
}
