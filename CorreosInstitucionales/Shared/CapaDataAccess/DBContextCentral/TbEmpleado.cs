using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;

[Table("tbEmpleados")]
public partial class TbEmpleado
{
    [Key]
    [Column("idEmpleado")]
    public int IdEmpleado { get; set; }

    [Column("empIdUsuario")]
    public int EmpIdUsuario { get; set; }

    [Column("empNoEmpleado")]
    public int EmpNoEmpleado { get; set; }

    [Column("empIdAreaDepto")]
    public int EmpIdAreaDepto { get; set; }

    [Column("empRFC")]
    [StringLength(10)]
    [Unicode(false)]
    public string EmpRfc { get; set; } = null!;

    [Column("empRFCHomoClave")]
    [StringLength(3)]
    [Unicode(false)]
    public string EmpRfchomoClave { get; set; } = null!;

    [Column("empAntiguedad")]
    public int EmpAntiguedad { get; set; }

    [ForeignKey("EmpIdAreaDepto")]
    [InverseProperty("TbEmpleados")]
    public virtual CatAreasDepto EmpIdAreaDeptoNavigation { get; set; } = null!;

    [ForeignKey("EmpIdUsuario")]
    [InverseProperty("TbEmpleados")]
    public virtual TbUsuario EmpIdUsuarioNavigation { get; set; } = null!;
}
