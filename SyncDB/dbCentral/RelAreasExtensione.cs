using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("relAreasExtensiones")]
public partial class RelAreasExtensione
{
    [Key]
    public int IdAreaNumerosExtensiones { get; set; }

    [Column("numIdAreaDepto")]
    public int NumIdAreaDepto { get; set; }

    [Column("numIdExtension")]
    public int NumIdExtension { get; set; }

    [ForeignKey("NumIdAreaDepto")]
    [InverseProperty("RelAreasExtensiones")]
    public virtual CatAreasDepto NumIdAreaDeptoNavigation { get; set; } = null!;

    [ForeignKey("NumIdExtension")]
    [InverseProperty("RelAreasExtensiones")]
    public virtual CatExtension NumIdExtensionNavigation { get; set; } = null!;
}
