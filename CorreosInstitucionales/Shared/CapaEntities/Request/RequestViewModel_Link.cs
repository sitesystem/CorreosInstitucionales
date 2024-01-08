using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Link
    {
        [Key]
        public int IdLink { get; set; }

        [Column("linkNombre")]
        [StringLength(50)]
        public string LinkNombre { get; set; } = null!;

        [Column("linkEnlace")]
        [StringLength(200)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string LinkEnlace { get; set; } = null!;

        [Required]
        [Column("linkStatus")]
        public bool LinkStatus { get; set; }
    }
}
