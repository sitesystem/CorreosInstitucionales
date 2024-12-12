using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catExtension")]
public partial class CatExtension
{
    [Key]
    public int IdExtension { get; set; }

    [Column("extNumero")]
    public int ExtNumero { get; set; }

    [Column("extStatus")]
    public bool ExtStatus { get; set; }

    [InverseProperty("NumIdExtensionNavigation")]
    public virtual ICollection<RelAreasExtensione> RelAreasExtensiones { get; set; } = new List<RelAreasExtensione>();
}
