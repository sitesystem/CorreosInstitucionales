﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catAnuncios")]
public partial class McCatAnuncio
{
    [Key]
    public int IdAnuncio { get; set; }

    [Column("anuDescripcion")]
    [StringLength(100)]
    public string? AnuDescripcion { get; set; }

    [Column("anuArchivo")]
    [StringLength(255)]
    public string AnuArchivo { get; set; } = null!;

    [Column("anuVisibleDesde", TypeName = "datetime")]
    public DateTime? AnuVisibleDesde { get; set; }

    [Column("anuVisibleHasta", TypeName = "datetime")]
    public DateTime? AnuVisibleHasta { get; set; }

    [Column("anuStatus")]
    public bool AnuStatus { get; set; }
}