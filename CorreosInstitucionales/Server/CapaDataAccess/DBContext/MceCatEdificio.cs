using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatEdificio
{
    public int IdEdificio { get; set; }

    public string EdiNombreOficial { get; set; } = null!;

    public string? EdiNombreAlias { get; set; }

    public bool EdiStatus { get; set; }
}
