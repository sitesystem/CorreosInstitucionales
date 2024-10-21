using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

[Table("MC_catTiposPersonal")]
public partial class McCatTiposPersonal
{
    /// <summary>
    /// PK ID Único del Tipo de Personal
    /// </summary>
    [Key]
    public int IdTipoPersonal { get; set; }

    /// <summary>
    /// Nombre del Tipo de Personal
    /// </summary>
    [Column("tipoperNombre")]
    [StringLength(100)]
    [Unicode(false)]
    public string TipoperNombre { get; set; } = null!;

    /// <summary>
    /// Descripción detallada del Tipo de Personal
    /// </summary>
    [Column("tipoperDescripcion", TypeName = "text")]
    public string? TipoperDescripcion { get; set; }

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Column("tipoperStatus")]
    public bool TipoperStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("UsuIdTipoPersonalNavigation")]
    public virtual ICollection<MpTbUsuario> MpTbUsuarios { get; set; } = [];
}
