using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_Escuela
    {
        /// <summary>
        /// PK ID Único de la Escuela
        /// </summary>
        [Key]
        public int IdEscuela { get; set; }

        /// <summary>
        /// Nombre de la Imágen del Logo
        /// </summary>
        [Column("escLogo")]
        [StringLength(100)]
        public string? EscLogo { get; set; }

        /// <summary>
        /// Número de la Escuela
        /// </summary>
        [Column("escNoEscuela")]
        [StringLength(10)]
        public string? EscNoEscuela { get; set; }

        /// <summary>
        /// Nombre Largo de la Escuela
        /// </summary>
        [Column("escNombreLargo")]
        [StringLength(150)]
        public string? EscNombreLargo { get; set; }

        /// <summary>
        /// Nombre Corto de la Escuela
        /// </summary>
        [Column("escNombreCorto")]
        [StringLength(100)]
        public string? EscNombreCorto { get; set; }

        [Column("escFileNameAvisoPrivacidad")]
        [StringLength(300)]
        public string? EscFileNameAvisoPrivacidad { get; set; }

        [Column("escFechaFundacion", TypeName = "datetime")]
        public DateTime? EscFechaFundacion { get; set; }

        /// <summary>
        /// Estado (1 = Activo, 0 = Inactivo)
        /// </summary>
        [Column("escStatus")]
        public bool EscStatus { get; set; }
    }
}
