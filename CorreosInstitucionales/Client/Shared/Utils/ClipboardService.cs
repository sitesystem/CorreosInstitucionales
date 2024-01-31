
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CorreosInstitucionales.Client.Shared.Utils
{
    public class ClipboardService: IClipboardService
    {
        private readonly IJSRuntime _runtime;

        public ClipboardService(IJSRuntime runtime) 
        {
            _runtime = runtime;
        }

        async Task IClipboardService.CopyToClipboard(string text)

        {
            await _runtime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }
    }
}
