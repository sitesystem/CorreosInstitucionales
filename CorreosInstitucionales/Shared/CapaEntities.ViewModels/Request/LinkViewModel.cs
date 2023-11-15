using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class LinkViewModel
    {
        [Key]
        public int IdLink { get; set; }

        [Column("linkNombre")]
        [StringLength(200)]
        public string LinkNombre { get; set; } = null!;

        [Column("linkStatus")]
        public bool LinkStatus { get; set; }
    }
}
