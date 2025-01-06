using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Plantilla : McCatPlantillas
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo NOMBRE PLANTILLA requerido.")]
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

        public override string ToString()
        {
            return $"{IdPlantilla}\t{PlaNombre}";
        }
    }
}
