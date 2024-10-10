using CorreosInstitucionales.Shared.Constantes;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared
{
    public class FileAttachment<T>
    {
        public T Value { get; set; }
        public List<ContentData> Attachments { get; set; } = new();
        public FileAttachment(T v)
        {
            Value = v;
        }

        public async Task AttachFile(IBrowserFile file)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var fs = file.OpenReadStream(file.Size))
                {
                    await fs.CopyToAsync(ms);
                }

                Attachments.Add(new()
                {
                    ContentType = file.ContentType,
                    FileName = file.Name,
                    Bytes = ms.ToArray()
                });
            }
        }
    }
}
