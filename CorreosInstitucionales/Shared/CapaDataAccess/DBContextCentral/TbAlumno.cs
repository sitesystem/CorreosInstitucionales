using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("tbAlumnos")]
public partial class TbAlumno
{
    [Key]
    public int IdAlumno { get; set; }

    [Column("aluIdUsuario")]
    public int AluIdUsuario { get; set; }

    [Column("aluPreBoleta")]
    public int AluPreBoleta { get; set; }

    [Column("aluBoleta")]
    public int AluBoleta { get; set; }

    [Column("aluIdCarrera")]
    public int AluIdCarrera { get; set; }

    [Column("aluIdSemestre")]
    public int AluIdSemestre { get; set; }

    [Column("aluAñoIngreso")]
    public int AluAñoIngreso { get; set; }

    [Column("aluAñoEgreso")]
    public int AluAñoEgreso { get; set; }

    [ForeignKey("AluIdCarrera")]
    [InverseProperty("TbAlumnos")]
    public virtual CatCarrera AluIdCarreraNavigation { get; set; } = null!;

    [ForeignKey("AluIdSemestre")]
    [InverseProperty("TbAlumnos")]
    public virtual CatSemestre AluIdSemestreNavigation { get; set; } = null!;

    [ForeignKey("AluIdUsuario")]
    [InverseProperty("TbAlumnos")]
    public virtual TbUsuario AluIdUsuarioNavigation { get; set; } = null!;
}
