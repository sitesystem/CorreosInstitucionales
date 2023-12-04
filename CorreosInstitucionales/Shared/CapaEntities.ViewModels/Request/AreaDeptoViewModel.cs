using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class AreaDeptoViewModel
    {
        [Key]
        public int IdAreaDepto { get; set; }

        [Column("areNombre", TypeName = "text")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string? AreNombre { get; set; }

        [Column("areExtension")]
        [StringLength(20)]
        public string? AreExtension { get; set; }

        [Column("areIdEdificio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int AreIdEdificio { get; set; }

        [Column("areIdPiso")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int AreIdPiso { get; set; }

        [Column("areTitular")]
        [StringLength(300)]
        public string? AreTitular { get; set; }

        [Required]
        [Column("areStatus")]
        public bool? AreStatus { get; set; }

        [ForeignKey("AreIdEdificio")]
        [InverseProperty("MceCatAreasDeptos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public virtual EdificioViewModel? AreIdEdificioNavigation { get; set; } = null!;

        [ForeignKey("AreIdPiso")]
        [InverseProperty("MceCatAreasDeptos")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public virtual PisoViewModel? AreIdPisoNavigation { get; set; } = null!;

        //[InverseProperty("ExtIdAreaDeptoNavigation")]
        //public virtual ICollection<MceCatExtensione> MceCatExtensiones { get; set; } = new List<MceCatExtensione>();

        //[InverseProperty("UsuIdAreaDeptoNavigation")]
        //public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
    }
}
