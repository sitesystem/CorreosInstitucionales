using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Anuncio
    {
        [Key]
        public int IdAnuncio { get; set; }

        [Column("anuDescripcion")]
        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo DESCRIPCIÓN requerido.")]
        public string? AnuDescripcion { get; set; }

        [Column("anuArchivo")]
        [StringLength(255)]
        public string AnuArchivo { get; set; } = null!;

        [Column("anuEnlace")]
        [StringLength(300)]
        public string? AnuEnlace { get; set; }

        [Column("anuVisibleDesde", TypeName = "datetime")]
        public DateTime? AnuVisibleDesde { get; set; }

        [Column("anuVisibleHasta", TypeName = "datetime")]
        public DateTime? AnuVisibleHasta { get; set; }

        [Column("anuStatus")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public bool AnuStatus { get; set; }
    }
}
