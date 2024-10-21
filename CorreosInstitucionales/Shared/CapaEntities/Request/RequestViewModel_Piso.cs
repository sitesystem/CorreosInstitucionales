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
    public class RequestViewModel_Piso : McCatPiso
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo DESCRIPCIÓN requerido.")]
        public new string PisoDescripcion
        {
            get { return base.PisoDescripcion; }
            set { base.PisoDescripcion = value; }
        }
    }
}
