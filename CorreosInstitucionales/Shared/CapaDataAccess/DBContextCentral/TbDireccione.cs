using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("tbDirecciones")]
public partial class TbDireccione
{
    [Key]
    public int IdDireccion { get; set; }

    [Column("dirIdEntidadFederativa")]
    public int DirIdEntidadFederativa { get; set; }

    [Column("dirDelegacion")]
    [StringLength(50)]
    [Unicode(false)]
    public string DirDelegacion { get; set; } = null!;

    [Column("dirIdCodigoPostal")]
    public int DirIdCodigoPostal { get; set; }

    [Column("dirColonia")]
    [StringLength(50)]
    [Unicode(false)]
    public string DirColonia { get; set; } = null!;

    [ForeignKey("DirIdEntidadFederativa")]
    [InverseProperty("TbDirecciones")]
    public virtual CatEntidadesFederativa DirIdEntidadFederativaNavigation { get; set; } = null!;

    [InverseProperty("UsuIdDireccionNavigation")]
    public virtual ICollection<TbUsuario> TbUsuarios { get; set; } = new List<TbUsuario>();
}
