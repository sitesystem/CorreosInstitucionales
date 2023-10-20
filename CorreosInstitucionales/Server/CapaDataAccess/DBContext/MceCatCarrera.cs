using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.DBContext;

[Table("MCE_catCarreras")]
public partial class MceCatCarrera
{
    [Key]
    [Column("Id_Carrera")]
    public int IdCarrera { get; set; }

    [Column("carrNombre")]
    [StringLength(300)]
    public string CarrNombre { get; set; } = null!;

    [Column("carrClave")]
    [StringLength(20)]
    [Unicode(false)]
    public string? CarrClave { get; set; }

    [Column("carrStatus")]
    public bool CarrStatus { get; set; }

    [InverseProperty("UsuIdCarreraNavigation")]
    public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
}
