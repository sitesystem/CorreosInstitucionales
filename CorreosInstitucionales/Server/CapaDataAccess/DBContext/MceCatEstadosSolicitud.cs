using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catEstadosSolicitud")]
public partial class MceCatEstadosSolicitud
{
    [Key]
    public int IdEstadosSolicitud { get; set; }

    [Column("estsolNombre")]
    [StringLength(30)]
    [Unicode(false)]
    public string EstsolNombre { get; set; } = null!;

    [Column("estsolStatus")]
    public bool EstsolStatus { get; set; }
}
