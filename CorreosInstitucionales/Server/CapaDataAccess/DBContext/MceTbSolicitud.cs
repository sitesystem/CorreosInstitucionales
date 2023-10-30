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

    [Column("solIdEstadoSolicitud")]
    public int SolIdEstadoSolicitud { get; set; }

    [Column("solIdAreaDepto")]
    public int SolIdAreaDepto { get; set; }

    [Column("solIdUsuario")]
    public int SolIdUsuario { get; set; }

    [Column("solFechaHoraAlta", TypeName = "datetime")]
    public DateTime SolFechaHoraAlta { get; set; }
}
