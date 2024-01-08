using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catEstadosSolicitud")]
public partial class McCatEstadosSolicitud
{
    /// <summary>
    /// PK ID Único del Estado de la Solicitud
    /// </summary>
    [Key]
    public int IdEstadoSolicitud { get; set; }

    /// <summary>
    /// Estado de la Solicitud (1 - Levantado, 2 - Pendiente, 3 - En Proceso, 4 - Atendido)
    /// </summary>
    [Column("edosolNombreEstado")]
    [StringLength(50)]
    [Unicode(false)]
    public string EdosolNombreEstado { get; set; } = null!;

    /// <summary>
    /// Detalle del Estado de la Solicitud
    /// </summary>
    [Column("edosolDescripcion", TypeName = "text")]
    public string? EdosolDescripcion { get; set; }

    [JsonIgnore]
    [InverseProperty("SolIdEstadoSolicitudNavigation")]
    public virtual ICollection<MtTbSolicitudesTicket> MtTbSolicitudesTickets { get; set; } = new List<MtTbSolicitudesTicket>();
}
