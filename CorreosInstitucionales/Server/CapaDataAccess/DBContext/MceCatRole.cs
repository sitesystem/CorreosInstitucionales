using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catRoles")]
public partial class MceCatRole
{
    [Key]
    public int IdRol { get; set; }

    [Column("rolNombre")]
    [StringLength(100)]
    [Unicode(false)]
    public string RolNombre { get; set; } = null!;

    [Column("rolDescripcion", TypeName = "text")]
    public string? RolDescripcion { get; set; }

    [JsonIgnore]
    [InverseProperty("UsuIdRolNavigation")]
    public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
}
