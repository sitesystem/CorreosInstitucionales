using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catRoles")]
public partial class CatRole
{
    [Key]
    public int IdRol { get; set; }

    [Column("rolNombre")]
    [StringLength(20)]
    [Unicode(false)]
    public string RolNombre { get; set; } = null!;

    [Column("rolDescricpcion")]
    [StringLength(80)]
    [Unicode(false)]
    public string? RolDescricpcion { get; set; }

    [Column("rolStatus")]
    public bool RolStatus { get; set; }
}
