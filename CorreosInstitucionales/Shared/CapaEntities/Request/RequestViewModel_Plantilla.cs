using System.ComponentModel.DataAnnotations;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    internal class RequestViewModel_Plantilla : McCatPlantilla
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE requerido.")]
        public new string PlaNombre
        {
            get { return base.PlaNombre; }
            set { base.PlaNombre = value; }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo CONTENIDO requerido.")]
        public new string PlaContenido
        {
            get { return base.PlaContenido; }
            set { base.PlaContenido = value; }
        }
    }
}
