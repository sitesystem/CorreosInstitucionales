using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatPiso
{
    public int IdPiso { get; set; }

    public string PisoDescripcion { get; set; } = null!;

    public bool PisoStatus { get; set; }
}
