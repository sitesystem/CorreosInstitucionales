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
    public class RequestViewModel_TipoSolicitud : McCatTiposSolicitud
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo DESCRIPCIÓN DEL TIPO DE SOLICITUD requerido.")]
        public new string TiposolDescripcion
        {
            get { return base.TiposolDescripcion; }
            set { base.TiposolDescripcion = value; }
        }
    }
}
