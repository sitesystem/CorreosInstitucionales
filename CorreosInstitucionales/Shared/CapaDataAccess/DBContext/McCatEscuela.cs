using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

[Table("MC_catEscuela")]
public partial class McCatEscuela
{
    /// <summary>
    /// PK ID Único de la Escuela
    /// </summary>
    [Key]
    public int IdEscuela { get; set; }

    /// <summary>
    /// Nombre de la Imágen del Logo
    /// </summary>
    [Column("escLogo")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EscLogo { get; set; }

    /// <summary>
    /// Número de la Escuela
    /// </summary>
    [Column("escNoEscuela")]
    [StringLength(10)]
    [Unicode(false)]
    public string? EscNoEscuela { get; set; }

    /// <summary>
    /// Nombre Largo de la Escuela
    /// </summary>
    [Column("escNombreLargo")]
    [StringLength(150)]
    [Unicode(false)]
    public string? EscNombreLargo { get; set; }

    /// <summary>
    /// Nombre Corto de la Escuela
    /// </summary>
    [Column("escNombreCorto")]
    [StringLength(100)]
    [Unicode(false)]
    public string? EscNombreCorto { get; set; }

    [Column("escFileNameAvisoPrivacidad")]
    [StringLength(300)]
    [Unicode(false)]
    public string? EscFileNameAvisoPrivacidad { get; set; }

    [Column("escFechaFundacion", TypeName = "datetime")]
    public DateTime? EscFechaFundacion { get; set; }

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Column("escStatus")]
    public bool EscStatus { get; set; }
}
