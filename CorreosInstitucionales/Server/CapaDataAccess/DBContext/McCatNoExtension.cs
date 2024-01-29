using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catNoExtension")]
public partial class McCatNoExtension
{
    /// <summary>
    /// PK ID Único del Número de Extensión
    /// </summary>
    [Key]
    public int IdExtension { get; set; }

    /// <summary>
    /// Número de Extensión
    /// </summary>
    [Column("extNoExtension")]
    [StringLength(10)]
    [Unicode(false)]
    public string ExtNoExtension { get; set; } = null!;

    /// <summary>
    /// FK ID del Área / Departamento
    /// </summary>
    [Column("extIdAreaDepto")]
    public int? ExtIdAreaDepto { get; set; }

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Required]
    [Column("extStatus")]
    public bool? ExtStatus { get; set; }

    [ForeignKey("ExtIdAreaDepto")]
    [InverseProperty("McCatNoExtensions")]
    public virtual McCatAreasDepto? ExtIdAreaDeptoNavigation { get; set; }
}
