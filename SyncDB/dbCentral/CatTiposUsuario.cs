using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catTiposUsuarios")]
public partial class CatTiposUsuario
{
    [Key]
    public int IdTipoUsuario { get; set; }

    [Column("tipusuNombre")]
    [StringLength(100)]
    [Unicode(false)]
    public string? TipusuNombre { get; set; }

    [Column("tipusuDescripcion", TypeName = "text")]
    public string? TipusuDescripcion { get; set; }
}
