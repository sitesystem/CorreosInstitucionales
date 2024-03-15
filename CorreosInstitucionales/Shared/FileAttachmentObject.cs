using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class FileAttachmentObject<T>
    {
        public T Value { get; set; }
        public Dictionary<int, IBrowserFile?>  Files { get; set; }

        public FileAttachmentObject(T v, IBrowserFile? f, int filetype)
        {
            Value = v;
            Files = new();
        }

        public FileAttachmentObject(T v, Dictionary<int, IBrowserFile?> files)
        {
            Value = v;
            Files = files;
        }
    }
}
