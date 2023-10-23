using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class TipoSolicitudViewModel
    {
        /// <summary>
        /// ID del tipo de solicitud
        /// </summary>
        [Key]
        public int IdTipoSolicitud { get; set; }

        /// <summary>
        /// Descripcion del tipo de la solicitud
        /// </summary>
        [Column("tiposolDescripcion", TypeName = "text")]
        public string TiposolDescripcion { get; set; } = null!;

        [Column("tiposolStatus")]
        public bool TiposolStatus { get; set; }
    }
}
