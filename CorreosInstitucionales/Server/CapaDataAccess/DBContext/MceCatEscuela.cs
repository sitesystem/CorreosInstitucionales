using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catEscuelas")]
public partial class MceCatEscuela
{
    [Key]
    public int IdEscuela { get; set; }

    [Column("escNoEscuela")]
    public int? EscNoEscuela { get; set; }

    [Column("escNombreLargo")]
    [StringLength(150)]
    [Unicode(false)]
    public string? EscNombreLargo { get; set; }

    [Column("escNombreCorto")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EscNombreCorto { get; set; }

    [Column("escLogo")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EscLogo { get; set; }

    [Required]
    [Column("escStatus")]
    public bool? EscStatus { get; set; }
}
