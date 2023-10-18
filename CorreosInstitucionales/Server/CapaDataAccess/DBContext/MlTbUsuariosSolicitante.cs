using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MlTbUsuariosSolicitante
{
    public int IdUsuarioSolicitante { get; set; }

    public string UsuNombre { get; set; } = null!;
}
