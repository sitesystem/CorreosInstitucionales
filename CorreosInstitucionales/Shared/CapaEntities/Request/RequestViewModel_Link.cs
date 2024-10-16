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
    public class RequestViewModel_Link :McCatLink
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public new string LinkEnlace
        {
            get { return base.LinkEnlace; }
            set { base.LinkEnlace = value; }
        }
    }
}
