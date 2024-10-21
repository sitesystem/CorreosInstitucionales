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
    public class RequestViewModel_TipoPersonal : McCatTiposPersonal
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo TIPO PERSONAL requerido.")]
        public new string TipoperNombre
        {
            get { return base.TipoperNombre; }
            set { base.TipoperNombre = value; }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo DESCRIPCIÓN DEL TIPO PERSONAL requerido.")]
        public new string? TipoperDescripcion
        {
            get { return base.TipoperDescripcion; }
            set { base.TipoperDescripcion = value; }
        }
    }
}
