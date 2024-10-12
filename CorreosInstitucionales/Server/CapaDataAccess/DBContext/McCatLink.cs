using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catLinks")]
public partial class McCatLink
{
    /// <summary>
    /// PK ID Único del Enlace Link
    /// </summary>
    [Key]
    public int IdLink { get; set; }

    /// <summary>
    /// Nombre del Enlace o Link
    /// </summary>
    [Column("linkNombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string LinkNombre { get; set; } = null!;

    /// <summary>
    /// Enlace o Link
    /// </summary>
    [Column("linkEnlace")]
    [StringLength(200)]
    [Unicode(false)]
    public string LinkEnlace { get; set; } = null!;

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Column("linkStatus")]
    public bool LinkStatus { get; set; }
}
