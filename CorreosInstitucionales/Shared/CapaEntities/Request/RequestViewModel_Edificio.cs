using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Edificio
    {
        [Key]
        public int IdEdificio { get; set; }

        [Column("ediNombreOficial")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string EdiNombreOficial { get; set; } = null!;

        [Column("ediNombreAlias")]
        [StringLength(200)]
        public string? EdiNombreAlias { get; set; }

        [Column("ediStatus")]
        public bool EdiStatus { get; set; }

        //[InverseProperty("AreIdEdificioNavigation")]
        //public virtual ICollection<MceCatAreasDepto> MceCatAreasDeptos { get; set; } = new List<MceCatAreasDepto>();
    }
}
