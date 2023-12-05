using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catAreasDeptos")]
public partial class MceCatAreasDepto
{
    [Key]
    public int IdAreaDepto { get; set; }

    [Column("areNombre", TypeName = "text")]
    public string? AreNombre { get; set; }

    [Column("areExtension")]
    [StringLength(20)]
    [Unicode(false)]
    public string? AreExtension { get; set; }

    [Column("areIdEdificio")]
    public int AreIdEdificio { get; set; }

    [Column("areIdPiso")]
    public int AreIdPiso { get; set; }

    [Column("areTitular")]
    [StringLength(300)]
    [Unicode(false)]
    public string? AreTitular { get; set; }

    [Required]
    [Column("areStatus")]
    public bool? AreStatus { get; set; }

    [ForeignKey("AreIdEdificio")]
    [InverseProperty("MceCatAreasDeptos")]
    public virtual MceCatEdificio? AreIdEdificioNavigation { get; set; } = null!;

    [ForeignKey("AreIdPiso")]
    [InverseProperty("MceCatAreasDeptos")]
    public virtual MceCatPiso? AreIdPisoNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("ExtIdAreaDeptoNavigation")]
    public virtual ICollection<MceCatExtension> MceCatExtensiones { get; set; } = new List<MceCatExtension>();

    [JsonIgnore]
    [InverseProperty("UsuIdAreaDeptoNavigation")]
    public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
}
