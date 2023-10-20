using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catEdificios")]
public partial class MceCatEdificio
{
    [Key]
    public int IdEdificio { get; set; }

    [Column("ediNombreOficial")]
    [StringLength(200)]
    [Unicode(false)]
    public string EdiNombreOficial { get; set; } = null!;

    [Column("ediNombreAlias")]
    [StringLength(200)]
    [Unicode(false)]
    public string? EdiNombreAlias { get; set; }

    [Column("ediStatus")]
    public bool EdiStatus { get; set; }

    [InverseProperty("AreIdEdificioNavigation")]
    public virtual ICollection<MceCatAreasDepto> MceCatAreasDeptos { get; set; } = new List<MceCatAreasDepto>();
}
