using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catPlantillas")]
public partial class McCatPlantilla
{
    [Key]
    public int IdPlantilla { get; set; }

    [Column("plaNombre")]
    [StringLength(100)]
    public string PlaNombre { get; set; } = null!;

    [Column("plaContenido")]
    [StringLength(2048)]
    public string PlaContenido { get; set; } = null!;

    [Column("plaStatus")]
    public bool PlaStatus { get; set; }
}
