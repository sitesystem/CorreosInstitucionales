using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("catAreasDeptos")]
public partial class CatAreasDepto
{
    /// <summary>
    /// PK ID Único del Área/Departamento
    /// </summary>
    [Key]
    public int IdAreaDepto { get; set; }

    /// <summary>
    /// Nombre del Área/Departamento
    /// </summary>
    [Column("areNombreAreaDepto")]
    [StringLength(200)]
    [Unicode(false)]
    public string AreNombreAreaDepto { get; set; } = null!;

    /// <summary>
    /// Jefe(a) del Área/Departamento
    /// </summary>
    [Column("areNombreJefe")]
    [StringLength(200)]
    [Unicode(false)]
    public string AreNombreJefe { get; set; } = null!;

    /// <summary>
    /// Titular responsable del Área/Departamento
    /// </summary>
    [Column("areTitularResponsable")]
    [StringLength(200)]
    [Unicode(false)]
    public string? AreTitularResponsable { get; set; }

    /// <summary>
    /// Correo Electrónico Administrativo del Área/Departamento
    /// </summary>
    [Column("areCorreoAreaDepto")]
    [StringLength(100)]
    [Unicode(false)]
    public string? AreCorreoAreaDepto { get; set; }

    /// <summary>
    /// FK ID Foránea del Edificio
    /// </summary>
    [Column("areIdEdificio")]
    public int AreIdEdificio { get; set; }

    /// <summary>
    /// FK ID Foránea del Piso
    /// </summary>
    [Column("areIdPiso")]
    public int AreIdPiso { get; set; }

    /// <summary>
    /// Estatus del Área/Departamento { 1 = Está en función, 0 = No está en función }
    /// </summary>
    [Column("areStatus")]
    public bool AreStatus { get; set; }

    [ForeignKey("AreIdEdificio")]
    [InverseProperty("CatAreasDeptos")]
    public virtual CatEdificio AreIdEdificioNavigation { get; set; } = null!;

    [ForeignKey("AreIdPiso")]
    [InverseProperty("CatAreasDeptos")]
    public virtual CatPiso AreIdPisoNavigation { get; set; } = null!;

    [InverseProperty("NumIdAreaDeptoNavigation")]
    public virtual ICollection<RelAreasExtensione> RelAreasExtensiones { get; set; } = new List<RelAreasExtensione>();

    [InverseProperty("EmpIdAreaDeptoNavigation")]
    public virtual ICollection<TbEmpleado> TbEmpleados { get; set; } = new List<TbEmpleado>();
}
