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
    public class RequestViewModel_Edificio : McCatEdificio
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE OFICIAL DEL EDIFICIO requerido.")]
        public new string EdiNombreOficial
        {
            get { return base.EdiNombreOficial; }
            set { base.EdiNombreOficial = value; }
        }
    }
}
