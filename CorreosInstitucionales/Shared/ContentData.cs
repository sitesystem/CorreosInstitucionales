using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class ContentData
    {
        public string ContentType { get; set; } = "";
        public byte[] Bytes { get; set; } = [];
        public string FileName { get; set; } = "UNDEFINED";
    }
}
