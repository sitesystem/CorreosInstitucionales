using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catTipoPersonal")]
public partial class MceCatTipoPersonal
{
    [Key]
    public int IdTipoPersonal { get; set; }

    [Column("tipoperNombre")]
    [StringLength(100)]
    [Unicode(false)]
    public string TipoperNombre { get; set; } = null!;

    [Column("tipoperDescripcion", TypeName = "text")]
    public string? TipoperDescripcion { get; set; }

    [Required]
    [Column("tipoperStatus")]
    public bool? TipoperStatus { get; set; }
}
