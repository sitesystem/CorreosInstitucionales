using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catEdificios")]
public partial class CatEdificio
{
    [Key]
    public int IdEdificio { get; set; }

    [Column("edifNombreOficial")]
    [StringLength(50)]
    [Unicode(false)]
    public string EdifNombreOficial { get; set; } = null!;

    [Column("edifNombreAlias")]
    [StringLength(50)]
    [Unicode(false)]
    public string EdifNombreAlias { get; set; } = null!;

    [Column("edifStatus")]
    public bool EdifStatus { get; set; }

    [InverseProperty("AreIdEdificioNavigation")]
    public virtual ICollection<CatAreasDepto> CatAreasDeptos { get; set; } = new List<CatAreasDepto>();
}
