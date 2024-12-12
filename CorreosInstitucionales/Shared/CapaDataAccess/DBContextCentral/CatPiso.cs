using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catPisos")]
public partial class CatPiso
{
    [Key]
    public int IdPiso { get; set; }

    [Column("pisoNombre")]
    [StringLength(15)]
    [Unicode(false)]
    public string PisoNombre { get; set; } = null!;

    [Column("pisoStatus")]
    public bool PisoStatus { get; set; }

    [InverseProperty("AreIdPisoNavigation")]
    public virtual ICollection<CatAreasDepto> CatAreasDeptos { get; set; } = new List<CatAreasDepto>();
}
