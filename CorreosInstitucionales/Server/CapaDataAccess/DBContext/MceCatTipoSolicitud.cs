using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catTipoSolicitud")]
public partial class MceCatTipoSolicitud
{
    /// <summary>
    /// ID del tipo de solicitud
    /// </summary>
    [Key]
    public int IdTipoSolicitud { get; set; }

    /// <summary>
    /// Descripcion del tipo de la solicitud
    /// </summary>
    [Column("tiposolDescripcion", TypeName = "text")]
    public string TiposolDescripcion { get; set; } = null!;

    [Required]
    [Column("tiposolStatus")]
    public bool? TiposolStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("SolIdTipoSolicitudNavigation")]
    public virtual ICollection<MceTbSolicitudTicket> MceTbSolicitudTickets { get; set; } = new List<MceTbSolicitudTicket>();
}
