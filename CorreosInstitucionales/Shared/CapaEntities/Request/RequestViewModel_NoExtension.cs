using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_NoExtension : McCatNoExtension
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE OFICIAL DEL EDIFICIO requerido.")]
        public new string ExtNoExtension
        {
            get { return base.ExtNoExtension; }
            set { base.ExtNoExtension = value; }
        }
    }
}
