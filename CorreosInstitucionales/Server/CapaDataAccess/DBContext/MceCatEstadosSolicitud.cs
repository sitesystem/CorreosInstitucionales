using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catEstadosSolicitud")]
public partial class MceCatEstadosSolicitud
{
    [Key]
    public int IdEstadoSolicitud { get; set; }

    [Column("edosolNombreEstado")]
    [StringLength(50)]
    [Unicode(false)]
    public string EdosolNombreEstado { get; set; } = null!;

    [Column("edosolDescripcion", TypeName = "text")]
    public string? EdosolDescripcion { get; set; }

    [JsonIgnore]
    [InverseProperty("SolIdEstadoSolicitudNavigation")]
    public virtual ICollection<MceTbSolicitudTicket> MceTbSolicitudTickets { get; set; } = new List<MceTbSolicitudTicket>();
}
