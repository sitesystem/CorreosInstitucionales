using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_tbSolicitud")]
public partial class MceTbSolicitud
{
    [Key]
    public int IdSolicitud { get; set; }

    [Column("solIdTipoSolicitud")]
    public int SolIdTipoSolicitud { get; set; }

    [Column("solIdUsuario")]
    public int SolIdUsuario { get; set; }

    [Column("solCapturaEscaneoAntivirus")]
    [StringLength(150)]
    [Unicode(false)]
    public string SolCapturaEscaneoAntivirus { get; set; } = null!;

    [Column("solCapturaCuentaBloqueada")]
    [StringLength(150)]
    [Unicode(false)]
    public string SolCapturaCuentaBloqueada { get; set; } = null!;

    [Column("solCaprturaError")]
    [StringLength(150)]
    [Unicode(false)]
    public string SolCaprturaError { get; set; } = null!;

    [Column("solFechaHoraAlta", TypeName = "datetime")]
    public DateTime SolFechaHoraAlta { get; set; }

    [Column("solIdEstadoSolicitud")]
    public int SolIdEstadoSolicitud { get; set; }
}
