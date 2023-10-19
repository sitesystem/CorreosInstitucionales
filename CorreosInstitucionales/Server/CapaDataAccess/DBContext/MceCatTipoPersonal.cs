using System;
using System.Collections.Generic;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

public partial class MceCatTipoPersonal
{
    public int IdTipoPersonal { get; set; }

    public string TipoperNombre { get; set; } = null!;

    public bool TipoperStatus { get; set; }

    public string? TipoperDescripcion { get; set; }

    public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
}
