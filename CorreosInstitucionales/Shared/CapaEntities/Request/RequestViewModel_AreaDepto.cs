using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_AreaDepto : McCatAreasDepto
    {
        /// <summary>
        /// Nombre del Área / Departamento
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo ÁREA/DEPARTAMENTO requerido.")]
        public new string AreNombreAreaDepto
        {
            get { return base.AreNombreAreaDepto; }
            set { base.AreNombreAreaDepto = value; }
        }

        /// <summary>
        /// FK ID del Edificio
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo EDIFICIO requerido.")]
        public new int AreIdEdificio
        {
            get { return base.AreIdEdificio; }
            set { base.AreIdEdificio = value; }
        }

        /// <summary>
        /// FK ID del Piso
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo PISO requerido.")]
        public new int AreIdPiso
        {
            get { return base.AreIdPiso; }
            set { base.AreIdPiso = value; }
        }
    }
}
