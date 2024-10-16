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
    public class RequestViewModel_Rol:McCatRole
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE requerido.")]
        public new string RolNombre
        {
            get { return base.RolNombre; }
            set { base.RolNombre = value; }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo DESCRIPCIÓN requerido.")]
        public new string? RolDescripcion
        {
            get { return base.RolDescripcion; }
            set { base.RolDescripcion = value; }
        }
    }
}
