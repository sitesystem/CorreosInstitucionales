using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catCarreras")]
public partial class MceCatCarrera
{
    [Key]
    public int IdCarrera { get; set; }

    [Column("carrClave")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CarrClave { get; set; }

    [Column("carrNombre")]
    [StringLength(300)]
    [Unicode(false)]
    public string CarrNombre { get; set; } = null!;

    [Required]
    [Column("carrStatus")]
    public bool? CarrStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("UsuIdCarreraNavigation")]
    public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
}
