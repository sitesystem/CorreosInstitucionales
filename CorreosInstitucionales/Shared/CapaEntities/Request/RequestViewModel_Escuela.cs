using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Escuela
    {
        [Key]
        public int IdEscuela { get; set; }

        [Column("escNoEscuela")]
        [StringLength(10)]
        public string? EscNoEscuela { get; set; }

        [Column("escNombreLargo")]
        [StringLength(150)]
        public string? EscNombreLargo { get; set; }

        [Column("escNombreCorto")]
        [StringLength(100)]
        public string? EscNombreCorto { get; set; }

        [Column("escLogo")]
        [StringLength(100)]
        public string? EscLogo { get; set; }

        [Required]
        [Column("escStatus")]
        public bool EscStatus { get; set; }
    }
}
