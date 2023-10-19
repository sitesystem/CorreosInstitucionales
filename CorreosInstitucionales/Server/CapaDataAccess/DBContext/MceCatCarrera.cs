using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatCarrera
{
    public int IdCarrera { get; set; }

    public string CarrNombre { get; set; } = null!;

    public bool CarrStatus { get; set; }
}
