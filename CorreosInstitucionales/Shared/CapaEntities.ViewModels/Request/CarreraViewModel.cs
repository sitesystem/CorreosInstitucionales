using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class CarreraViewModel
    {
        [Key]
        [Column("Id_Carrera")]
        public int IdCarrera { get; set; }

        [Column("carrNombre")]
        [StringLength(300)]
        public string CarrNombre { get; set; } = null!;

        [Column("carrClave")]
        [StringLength(20)]
        public string? CarrClave { get; set; }

        [Column("carrStatus")]
        public bool CarrStatus { get; set; }

    }
}
