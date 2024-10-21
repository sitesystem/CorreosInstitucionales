using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

[Table("MC_catAnuncios")]
public partial class McCatAnuncio
{
    /// <summary>
    /// PK ID Único del Anuncio
    /// </summary>
    [Key]
    public int IdAnuncio { get; set; }

    /// <summary>
    /// Descripción i información del Anuncio
    /// </summary>
    [Column("anuDescripcion")]
    [StringLength(100)]
    public string? AnuDescripcion { get; set; }

    /// <summary>
    /// Nombre del Archivo en formato .jpg, .png
    /// </summary>
    [Column("anuArchivo")]
    [StringLength(255)]
    public string AnuArchivo { get; set; } = null!;

    /// <summary>
    /// Enlace del Anuncio ya sea del servidor o dirección URL
    /// </summary>
    [Column("anuEnlace")]
    [StringLength(300)]
    public string? AnuEnlace { get; set; }

    /// <summary>
    /// Fecha de Inicio del periodo del Anuncio
    /// </summary>
    [Column("anuVisibleDesde", TypeName = "datetime")]
    public DateTime? AnuVisibleDesde { get; set; }

    /// <summary>
    /// Fecha de Fin del periodo del Anuncio
    /// </summary>
    [Column("anuVisibleHasta", TypeName = "datetime")]
    public DateTime? AnuVisibleHasta { get; set; }

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Column("anuStatus")]
    public bool AnuStatus { get; set; }
}
