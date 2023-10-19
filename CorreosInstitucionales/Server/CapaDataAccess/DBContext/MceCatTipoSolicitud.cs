using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatTipoSolicitud
{
    /// <summary>
    /// ID del tipo de solicitud
    /// </summary>
    public int IdTipoSolicitud { get; set; }

    /// <summary>
    /// Descripcion del tipo de la solicitud
    /// </summary>
    public string TiposolDescripcion { get; set; } = null!;

    public bool? TiposolStatus { get; set; }
}
