using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceTbSolicitud
{
    public int IdSolicitud { get; set; }

    public int SolIdTipoSolicitud { get; set; }

    public int SolIdEstadosSolicitud { get; set; }

    public int? SolIdAreaDepto { get; set; }

    public int SolIdUsuario { get; set; }

    public DateTime SolFecha { get; set; }
}
