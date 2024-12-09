using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_SendWhatsApp
    {
        public string Number { get; set; } = null!;
        public string Message { get; set; } = null!;

        public override string ToString()
        {
            string trimmed = Message!.Length > 32 ? string.Join(" ", Message!.Substring(0, 32).Split(' ').Skip(1)) + "..." : Message;
            return $"{Number}\t{trimmed}";
        }
    }
}
