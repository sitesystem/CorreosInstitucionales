using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Semestre
    {
        [Key]
        public int IdSemestre { get; set; }

        [Column("semNombre")]
        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo SEMESTRE requerido.")]
        public string SemNombre { get; set; } = null!;
    }
}
