using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_ChangePassword
    {
        public int IdUsuario { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NUEVA CONTRASEÑA requerido.")]
        public string NewPassword { get; set; } = null!;
    }
}
