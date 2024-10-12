using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catAreasDeptos")]
public partial class McCatAreasDepto
{
    /// <summary>
    /// PK ID Único del Área / Departamento
    /// </summary>
    [Key]
    public int IdAreaDepto { get; set; }

    /// <summary>
    /// Nombre del Área / Departamento
    /// </summary>
    [Column("areNombreAreaDepto")]
    [StringLength(300)]
    public string AreNombreAreaDepto { get; set; } = null!;

    /// <summary>
    /// Números de Extensión relacionados al Área / Departamento
    /// </summary>
    [Column("areNoExtension")]
    [StringLength(100)]
    [Unicode(false)]
    public string? AreNoExtension { get; set; }

    /// <summary>
    /// FK ID del Edificio
    /// </summary>
    [Column("areIdEdificio")]
    public int AreIdEdificio { get; set; }

    /// <summary>
    /// FK ID del Piso
    /// </summary>
    [Column("areIdPiso")]
    public int AreIdPiso { get; set; }

    /// <summary>
    /// Titular responsable del Área / Departamento
    /// </summary>
    [Column("areTitular")]
    [StringLength(300)]
    [Unicode(false)]
    public string? AreTitular { get; set; }

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Column("areStatus")]
    public bool AreStatus { get; set; }

    [ForeignKey("AreIdEdificio")]
    [InverseProperty("McCatAreasDeptos")]
    public virtual McCatEdificio AreIdEdificioNavigation { get; set; } = null!;

    [ForeignKey("AreIdPiso")]
    [InverseProperty("McCatAreasDeptos")]
    public virtual McCatPiso AreIdPisoNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("ExtIdAreaDeptoNavigation")]
    public virtual ICollection<McCatNoExtension> McCatNoExtensions { get; set; } = new List<McCatNoExtension>();

    [JsonIgnore]
    [InverseProperty("UsuIdAreaDeptoNavigation")]
    public virtual ICollection<MpTbUsuario> MpTbUsuarios { get; set; } = new List<MpTbUsuario>();
}
