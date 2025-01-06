using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catSemestres")]
public partial class CatSemestre
{
    [Key]
    public int IdSemestre { get; set; }

    /// <summary>
    /// Último Semestre Cursado del Alumn@
    /// </summary>
    [Column("semNivelSemestre")]
    [StringLength(10)]
    [Unicode(false)]
    public string SemNivelSemestre { get; set; } = null!;

    [InverseProperty("AluIdSemestreNavigation")]
    public virtual ICollection<TbAlumno> TbAlumnos { get; set; } = new List<TbAlumno>();
}
