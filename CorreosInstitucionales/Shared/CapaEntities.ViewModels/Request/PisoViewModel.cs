using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class PisoViewModel
    {
        [Key]
        public int IdPiso { get; set; }

        [Column("pisoDescripcion")]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string PisoDescripcion { get; set; } = null!;

        [Column("pisoStatus")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public bool PisoStatus { get; set; }

        //[InverseProperty("AreIdPisoNavigation")]
        //public virtual ICollection<MceCatAreasDepto> MceCatAreasDeptos { get; set; } = new List<MceCatAreasDepto>();
    }
}
