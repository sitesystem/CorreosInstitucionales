using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatRole
{
    public int IdRol { get; set; }

    public string RolNombre { get; set; } = null!;

    public string? RolDescripcion { get; set; }
}
