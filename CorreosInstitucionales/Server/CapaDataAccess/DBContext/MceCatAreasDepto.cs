using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatAreasDepto
{
    public int IdAreaDepto { get; set; }

    public string AreNombre { get; set; } = null!;

    public string? AreExtension { get; set; }

    public int AreIdEdificio { get; set; }

    public int AreIdPiso { get; set; }

    public bool AreStatus { get; set; }
}
