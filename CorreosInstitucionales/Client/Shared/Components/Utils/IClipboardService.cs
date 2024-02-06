using Microsoft.AspNetCore.Components;

namespace CorreosInstitucionales.Client.Shared.Components.Utils
{
    public interface IClipboardService
    {
        Task CopyToClipboard(string text);
    }
}
