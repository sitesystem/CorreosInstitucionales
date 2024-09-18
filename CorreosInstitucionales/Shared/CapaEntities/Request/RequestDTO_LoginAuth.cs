using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_LoginAuth
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        //[RegularExpression("^[A-Za-z0-9._%+-]*@ipn.mx$", ErrorMessage = "Formato Incorrecto.")]
        public string UsuCorreoPersonal { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string UsuContrasenia { get; set; } = null!;
    }
}
