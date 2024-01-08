using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestViewModel_AreaDepto
    {
        /// <summary>
        /// PK ID Único del Área / Departamento
        /// </summary>
        [Key]
        public int IdAreaDepto { get; set; }

        /// <summary>
        /// Nombre del Área / Departamento
        /// </summary>
        [Column("areNombreAreaDepto", TypeName = "text")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public string AreNombreAreaDepto { get; set; } = null!;

        /// <summary>
        /// Números de Extensión relacionados al Área / Departamento
        /// </summary>
        [Column("areNoExtension")]
        [StringLength(100)]
        public string? AreNoExtension { get; set; }

        /// <summary>
        /// FK ID del Edificio
        /// </summary>
        [Column("areIdEdificio")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int AreIdEdificio { get; set; }

        /// <summary>
        /// FK ID del Piso
        /// </summary>
        [Column("areIdPiso")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo requerido.")]
        public int AreIdPiso { get; set; }

        /// <summary>
        /// Titular responsable del Área / Departamento
        /// </summary>
        [Column("areTitular")]
        [StringLength(300)]
        public string? AreTitular { get; set; }

        /// <summary>
        /// Estado (1 = Activo, 0 = Inactivo)
        /// </summary>
        [Required]
        [Column("areStatus")]
        public bool AreStatus { get; set; }

        [ForeignKey("AreIdEdificio")]
        [InverseProperty("McCatAreasDeptos")]
        public virtual RequestViewModel_Edificio? AreIdEdificioNavigation { get; set; } = null!;

        [ForeignKey("AreIdPiso")]
        [InverseProperty("McCatAreasDeptos")]
        public virtual RequestViewModel_Piso? AreIdPisoNavigation { get; set; } = null!;

        //[JsonIgnore]
        //[InverseProperty("ExtIdAreaDeptoNavigation")]
        //public virtual ICollection<McCatNoExtension> McCatNoExtensions { get; set; } = new List<McCatNoExtension>();

        //[JsonIgnore]
        //[InverseProperty("UsuIdAreaDeptoNavigation")]
        //public virtual ICollection<MpTbUsuario> MpTbUsuarios { get; set; } = new List<MpTbUsuario>();
    }
}
