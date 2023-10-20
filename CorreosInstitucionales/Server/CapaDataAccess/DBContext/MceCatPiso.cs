using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catPisos")]
public partial class MceCatPiso
{
    [Key]
    public int IdPiso { get; set; }

    [Column("pisoDescripcion")]
    [StringLength(50)]
    public string PisoDescripcion { get; set; } = null!;

    [Column("pisoStatus")]
    public bool PisoStatus { get; set; }

    [InverseProperty("AreIdPisoNavigation")]
    public virtual ICollection<MceCatAreasDepto> MceCatAreasDeptos { get; set; } = new List<MceCatAreasDepto>();
}
