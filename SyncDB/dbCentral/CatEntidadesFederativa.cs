using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catEntidadesFederativas")]
public partial class CatEntidadesFederativa
{
    [Key]
    public int IdEntidadFederativa { get; set; }

    [Column("efNombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string EfNombre { get; set; } = null!;

    [InverseProperty("DirIdEntidadFederativaNavigation")]
    public virtual ICollection<TbDireccione> TbDirecciones { get; set; } = new List<TbDireccione>();
}
