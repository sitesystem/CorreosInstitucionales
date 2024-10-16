using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

[Table("MC_catEdificios")]
public partial class McCatEdificio
{
    /// <summary>
    /// PK ID Único del Edificio
    /// </summary>
    [Key]
    public int IdEdificio { get; set; }

    /// <summary>
    /// Nombre Oficial del Edificio
    /// </summary>
    [Column("ediNombreOficial")]
    [StringLength(200)]
    [Unicode(false)]
    public string EdiNombreOficial { get; set; } = null!;

    /// <summary>
    /// Nombre Alias del Edificio
    /// </summary>
    [Column("ediNombreAlias")]
    [StringLength(200)]
    [Unicode(false)]
    public string? EdiNombreAlias { get; set; }

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Column("ediStatus")]
    public bool EdiStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("AreIdEdificioNavigation")]
    public virtual ICollection<McCatAreasDepto> McCatAreasDeptos { get; set; } = new List<McCatAreasDepto>();
}
