using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class TipoPersonalViewModel
    {
        [Key]
        public int IdTipoPersonal { get; set; }

        [Column("tipoperNombre")]
        [StringLength(100)]
        public string TipoperNombre { get; set; } = null!;

        [Column("tipoperDescripcion", TypeName = "text")]
        public string? TipoperDescripcion { get; set; }

        [Required]
        [Column("tipoperStatus")]
        public bool? TipoperStatus { get; set; }

        //[InverseProperty("UsuIdTipoPersonalNavigation")]
        //public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
    }
}
