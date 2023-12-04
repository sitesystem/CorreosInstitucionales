using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catExtensiones")]
public partial class MceCatExtensione
{
    [Key]
    public int IdExtension { get; set; }

    [Column("extNoExtension")]
    [StringLength(10)]
    [Unicode(false)]
    public string ExtNoExtension { get; set; } = null!;

    [Column("extIdAreaDepto")]
    public int? ExtIdAreaDepto { get; set; }

    [Required]
    [Column("extStatus")]
    public bool? ExtStatus { get; set; }

    [ForeignKey("ExtIdAreaDepto")]
    [InverseProperty("MceCatExtensiones")]
    public virtual MceCatAreasDepto? ExtIdAreaDeptoNavigation { get; set; }
}
