using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catEscuelas")]
public partial class CatEscuela
{
    [Key]
    public int IdEscuela { get; set; }

    [Column("escLogo")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EscLogo { get; set; }

    [Column("escNoEscuela")]
    [StringLength(10)]
    [Unicode(false)]
    public string? EscNoEscuela { get; set; }

    [Column("escNombreLargo")]
    [StringLength(150)]
    [Unicode(false)]
    public string? EscNombreLargo { get; set; }

    [Column("escNombreCorto")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EscNombreCorto { get; set; }

    [Column("escAvisoPrivacidad")]
    [Unicode(false)]
    public string? EscAvisoPrivacidad { get; set; }

    [Column("escFechaFundacion", TypeName = "datetime")]
    public DateTime? EscFechaFundacion { get; set; }

    [Column("escStatus")]
    public bool? EscStatus { get; set; }
}
