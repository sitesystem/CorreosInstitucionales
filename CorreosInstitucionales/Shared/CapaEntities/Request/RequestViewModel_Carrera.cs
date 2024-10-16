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
    public class RequestViewModel_Carrera : McCatCarrera
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public new string? CarrClave
        {
            get { return base.CarrClave; }
            set { base.CarrClave = value; }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public new string CarrNombre
        {
            get { return base.CarrNombre; }
            set { base.CarrNombre = value; }
        }
    }
}
