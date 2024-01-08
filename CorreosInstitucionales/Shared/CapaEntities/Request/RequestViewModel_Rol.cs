using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Rol
    {
        [Key]
        public int IdRol { get; set; }

        [Column("rolNombre")]
        [StringLength(100)]
        public string RolNombre { get; set; } = null!;

        [Column("rolDescripcion", TypeName = "text")]
        public string? RolDescripcion { get; set; }

        //[InverseProperty("UsuIdRolNavigation")]
        //public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
    }
}
