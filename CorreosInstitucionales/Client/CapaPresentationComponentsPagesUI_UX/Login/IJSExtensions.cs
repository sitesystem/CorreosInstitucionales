using Microsoft.JSInterop;

namespace CorreosInstitucionales.Client.CapaPresentationComponentsPagesUI_UX.Login
{
    public class IJSExtensions(IJSRuntime js)
    {
        private readonly IJSRuntime _js = js;

        public ValueTask<object> SetInLocalStorage(string key, string content)
        {
            return _js.InvokeAsync<object>("localStorage.setItem", key, content);
        }

        public ValueTask<string> GetFromLocalStorage(string key)
        {
            return _js.InvokeAsync<string>("localStorage.getItem", key);
        }

        public ValueTask<object> RemoveItem(string key)
        {
            return _js.InvokeAsync<object>("localStorage.removeItem", key);
        }
    }
}
