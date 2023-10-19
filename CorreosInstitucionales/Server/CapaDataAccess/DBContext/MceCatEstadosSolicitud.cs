using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatEstadosSolicitud
{
    public int IdEstadosSolicitud { get; set; }

    public string EstsolNombre { get; set; } = null!;

    public bool PisoStatus { get; set; }
}
