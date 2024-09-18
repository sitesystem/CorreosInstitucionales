using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catPisos")]
public partial class McCatPiso
{
    /// <summary>
    /// PK ID Único del Piso / Nivel
    /// </summary>
    [Key]
    public int IdPiso { get; set; }

    /// <summary>
    /// Nombre / Descripción del Piso
    /// </summary>
    [Column("pisoDescripcion")]
    [StringLength(50)]
    [Unicode(false)]
    public string PisoDescripcion { get; set; } = null!;

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Column("pisoStatus")]
    public bool PisoStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("AreIdPisoNavigation")]
    public virtual ICollection<McCatAreasDepto> McCatAreasDeptos { get; set; } = new List<McCatAreasDepto>();
}
