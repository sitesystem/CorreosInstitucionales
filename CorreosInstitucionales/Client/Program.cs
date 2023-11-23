using CorreosInstitucionales.Client;
using CorreosInstitucionales.Client.CapaPresentation_ComponentsPages_UI_UX;
using CorreosInstitucionales.Client.Shared;

using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catCarrerasService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEdificiosService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catLinksService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catPisosService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposPersonalService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposSolicitudService;

using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.tbUsuariosService;
using CorreosInstitucionales.Shared.CapaTools;


using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Syncfusion.Blazor;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddSingleton(sp => new HttpClient(httpClientHandler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Inyección de Dependencias - Módulo de Login
builder.Services.AddScoped<IEdificio, REdificio>();
builder.Services.AddScoped<IPiso, RPiso>();
builder.Services.AddScoped<ICarrera, RCarrera>();
builder.Services.AddScoped<ITipoSolicitud, RTipoSolicitud>();
builder.Services.AddScoped<ITipoPersonal, RTipoPersonal>();
builder.Services.AddScoped<IUsuario, RUsuario>();
builder.Services.AddScoped<ILink, RLink>();

builder.Services.AddSyncfusionBlazor(options => { options.EnableRtl = false; options.Animation = GlobalAnimationMode.Enable; /*options.IgnoreScriptIsolation = true;*/ });
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjcwMzY4OUAzMjMzMmUzMDJlMzBUZHd6Sy8rUkNGSDAvQzNibGRkaXJhVmtZT0MrWlVrTmkvRFFFWW45bFBZPQ==");

// Register the Syncfusion locale service to localize Syncfusion Blazor components.
builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));

// Setting culture of the application
var host = builder.Build();
var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
var result = await jsInterop.InvokeAsync<string>("cultureInfo.get");
CultureInfo culture;
if (result != null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo("es");
    await jsInterop.InvokeVoidAsync("cultureInfo.set", "es");
}
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await builder.Build().RunAsync();
