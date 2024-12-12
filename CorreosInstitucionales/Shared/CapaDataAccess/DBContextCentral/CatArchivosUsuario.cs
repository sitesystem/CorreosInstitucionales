using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catArchivosUsuarios")]
public partial class CatArchivosUsuario
{
    [Key]
    public int IdTipoArchivo { get; set; }

    [Column("tipIdUsuario")]
    public int TipIdUsuario { get; set; }

    [Column("tipNombreArchivo")]
    [StringLength(50)]
    [Unicode(false)]
    public string TipNombreArchivo { get; set; } = null!;
}
