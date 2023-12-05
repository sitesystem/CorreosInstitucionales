using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class ExtensionViewModel
    {
        [Key]
        public int IdExtension { get; set; }

        [Column("extNoExtension")]
        [StringLength(10)]
        public string ExtNoExtension { get; set; } = null!;

        [Column("extIdAreaDepto")]
        public int? ExtIdAreaDepto { get; set; }

        [Required]
        [Column("extStatus")]
        public bool ExtStatus { get; set; }

        [ForeignKey("ExtIdAreaDepto")]
        [InverseProperty("MceCatExtensiones")]
        public virtual AreaDeptoViewModel? ExtIdAreaDeptoNavigation { get; set; }
    }
}
