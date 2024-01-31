using Microsoft.AspNetCore.Components;

namespace CorreosInstitucionales.Client.Shared.Utils
{
    public interface IClipboardService
    {
        Task CopyToClipboard(string text);
    }
}
