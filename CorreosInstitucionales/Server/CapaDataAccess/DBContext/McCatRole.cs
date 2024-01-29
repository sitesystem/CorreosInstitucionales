using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catRoles")]
public partial class McCatRole
{
    /// <summary>
    /// PK ID Único del Rol
    /// </summary>
    [Key]
    public int IdRol { get; set; }

    /// <summary>
    /// Nombre del Rol
    /// </summary>
    [Column("rolNombre")]
    [StringLength(100)]
    [Unicode(false)]
    public string RolNombre { get; set; } = null!;

    /// <summary>
    /// Descripción detallada del Rol
    /// </summary>
    [Column("rolDescripcion", TypeName = "text")]
    public string? RolDescripcion { get; set; }

    [JsonIgnore]
    [InverseProperty("UsuIdRolNavigation")]
    public virtual ICollection<MpTbUsuario> MpTbUsuarios { get; set; } = new List<MpTbUsuario>();
}
