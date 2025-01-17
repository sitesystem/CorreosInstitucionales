﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

[Table("MC_catPlantillas")]
public partial class McCatPlantillas
{
    [Key]
    public int IdPlantilla { get; set; }

    [Column("plaNombre")]
    [StringLength(100)]
    public string PlaNombre { get; set; } = null!;

    [Column("plaAsunto")]
    [StringLength(100)]
    public string PlaAsunto { get; set; } = null!;

    [Column("plaContenido")]
    [StringLength(2048)]
    public string PlaContenido { get; set; } = null!;

    [Column("plaTipo")]
    public int PlaTipo { get; set; }

    [Column("plaIdEstadoSolicitud")]
    public int? PlaIdEstadoSolicitud { get; set; }

    [Column("plaFiltro")]
    public int PlaFiltro { get; set; }

    [Column("plaStatus")]
    public bool PlaStatus { get; set; }
}
