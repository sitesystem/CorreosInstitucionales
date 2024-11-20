using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catTiposPersonalUDI")]
public partial class CatTiposPersonalUdi
{
    [Key]
    [Column("IdRolUDI")]
    public int IdRolUdi { get; set; }

    [Column("roludiNombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string? RoludiNombre { get; set; }

    [Column("roludiDescripcion")]
    [StringLength(300)]
    [Unicode(false)]
    public string? RoludiDescripcion { get; set; }

    [Column("roludiStatus")]
    public bool? RoludiStatus { get; set; }
}
