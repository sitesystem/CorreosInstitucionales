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
        public string? AreNombre { get; set; }

        [Column("areExtension")]
        [StringLength(20)]
        public string? AreExtension { get; set; }

        [Column("areIdEdificio")]
        public int? AreIdEdificio { get; set; }

        [Column("areIdPiso")]
        public int? AreIdPiso { get; set; }

        [Column("areTitular")]
        [StringLength(300)]
        public string? AreTitular { get; set; }

        [Column("areStatus")]
        public bool? AreStatus { get; set; }

        [ForeignKey("AreIdEdificio")]
        [InverseProperty("MceCatAreasDeptos")]
        public virtual EdificioViewModel? AreIdEdificioNavigation { get; set; }

        [ForeignKey("AreIdPiso")]
        [InverseProperty("MceCatAreasDeptos")]
        public virtual PisoViewModel? AreIdPisoNavigation { get; set; }
    }
}
