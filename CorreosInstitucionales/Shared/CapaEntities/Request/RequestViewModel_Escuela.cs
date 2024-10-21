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
    public class RequestViewModel_Escuela : McCatEscuela
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE OFICIAL DE LA ESCUELA requerido.")]
        public new string? EscNombreLargo
        {
            get { return base.EscNombreLargo; }
            set { base.EscNombreLargo = value; }
        }
    }
}
