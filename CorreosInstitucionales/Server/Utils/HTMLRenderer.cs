using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public class ComponentRenderer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILoggerFactory _loggerFactory;
        private readonly HtmlRenderer _renderer;

        public ComponentRenderer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            _renderer = new HtmlRenderer(_serviceProvider, _loggerFactory);
        }

        public async Task<string> GetHTML<Component>(Dictionary<string, object?>? data = null) where Component : IComponent
        {
            ParameterView? parameters = data is null ? null : ParameterView.FromDictionary(data);
            string? response = await _renderer.Dispatcher.InvokeAsync(async() => await Render<Component>(parameters));
            return response;
        }

        private async Task<string> Render<Component>(ParameterView? parameters) where Component : IComponent
        {
            HtmlRootComponent output;
            
            if(parameters is null)
            {
                output = await _renderer.RenderComponentAsync<Component>();
            }
            else
            {
                output = await _renderer.RenderComponentAsync<Component>(parameters.Value);
            }

            return output.ToHtmlString();
        }
    }
}
