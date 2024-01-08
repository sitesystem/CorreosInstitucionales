using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MC_catCarreras")]
public partial class McCatCarrera
{
    /// <summary>
    /// PK ID Único de la Carrera
    /// </summary>
    [Key]
    public int IdCarrera { get; set; }

    /// <summary>
    /// Clave de la Carrera / Licenciatura
    /// </summary>
    [Column("carrClave")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CarrClave { get; set; }

    /// <summary>
    /// Nombre de la Carrera / Licenciatura
    /// </summary>
    [Column("carrNombre")]
    [StringLength(300)]
    [Unicode(false)]
    public string CarrNombre { get; set; } = null!;

    /// <summary>
    /// Estado (1 = Activo, 0 = Inactivo)
    /// </summary>
    [Required]
    [Column("carrStatus")]
    public bool? CarrStatus { get; set; }

    [JsonIgnore]
    [InverseProperty("UsuIdCarreraNavigation")]
    public virtual ICollection<MpTbUsuario> MpTbUsuarios { get; set; } = new List<MpTbUsuario>();
}
