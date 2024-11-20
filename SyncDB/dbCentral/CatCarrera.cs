using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catCarreras")]
public partial class CatCarrera
{
    /// <summary>
    /// PK ID Único de la Carrera
    /// </summary>
    [Key]
    public int IdCarrera { get; set; }

    /// <summary>
    /// Clave o Código de la Carrera
    /// </summary>
    [Column("carrClave")]
    [StringLength(20)]
    [Unicode(false)]
    public string CarrClave { get; set; } = null!;

    /// <summary>
    /// Nombre de la Carrera / Especialidad
    /// </summary>
    [Column("carrCarreraEspecialidad")]
    [StringLength(100)]
    [Unicode(false)]
    public string CarrCarreraEspecialidad { get; set; } = null!;

    [Column("carrStatus")]
    public bool CarrStatus { get; set; }

    [InverseProperty("AluIdCarreraNavigation")]
    public virtual ICollection<TbAlumno> TbAlumnos { get; set; } = new List<TbAlumno>();
}
