using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catLinks")]
public partial class CatLink
{
    [Key]
    public int IdLink { get; set; }

    [Column("linkNombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string? LinkNombre { get; set; }

    [Column("linkEnlace")]
    [StringLength(200)]
    [Unicode(false)]
    public string? LinkEnlace { get; set; }

    [Column("linkStatus")]
    public bool? LinkStatus { get; set; }
}
