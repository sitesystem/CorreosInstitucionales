using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catTiposSolicitud")]
public partial class McCatTiposSolicitud
{
    /// <summary>
    /// PK ID Único del Tipo de Solicitud
    /// </summary>
    [Key]
    public int IdTipoSolicitud { get; set; }

    /// <summary>
    /// Descripción breve del Tipo de Solicitud
    /// </summary>
    [Column("tiposolDescripcion", TypeName = "text")]
    public string TiposolDescripcion { get; set; } = null!;

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Required]
    [Column("tiposolStatus")]
    public bool? TiposolStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("SolIdTipoSolicitudNavigation")]
    public virtual ICollection<MtTbSolicitudesTicket> MtTbSolicitudesTickets { get; set; } = new List<MtTbSolicitudesTicket>();
}
