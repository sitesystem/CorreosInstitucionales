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
    public class RequestViewModel_Semestre:McCatSemestre
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo SEMESTRE requerido.")]
        public new string SemNombre
        {
            get { return base.SemNombre; }
            set { base.SemNombre = value; }
        }
    }
}
