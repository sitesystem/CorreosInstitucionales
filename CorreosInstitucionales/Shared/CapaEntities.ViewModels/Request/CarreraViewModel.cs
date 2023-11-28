using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class CarreraViewModel
    {
        [Key]
        public int IdCarrera { get; set; }

        [Column("carrClave")]
        [StringLength(20)]
        public string? CarrClave { get; set; }

        [Column("carrNombre")]
        [StringLength(300)]
        public string CarrNombre { get; set; } = null!;

        [Required]
        [Column("carrStatus")]
        public bool CarrStatus { get; set; }

        //[JsonIgnore]
        //[InverseProperty("UsuIdCarreraNavigation")]
        //public virtual ICollection<MceTbUsuario> MceTbUsuarios { get; set; } = new List<MceTbUsuario>();
    }
}
