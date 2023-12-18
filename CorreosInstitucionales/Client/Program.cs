using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Syncfusion.Blazor;
using System.Globalization;

using CorreosInstitucionales.Client;
using CorreosInstitucionales.Client.Shared;
using CorreosInstitucionales.Client.CapaPresentation.ComponentsPages.UI_UX.Login;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catCarrerasService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEdificiosService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEscuelasService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catLinksService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catPisosService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposPersonalService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposSolicitudService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.tbUsuariosService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catAreasDeptosService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catExtensionService;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.SendEmailService;
using Syncfusion.Blazor.Popups;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddSingleton(sp => new HttpClient(httpClientHandler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Inyección de Dependencias - Módulo de Login
builder.Services.AddAuthorizationCore();
builder.Services.ConfigureAuthorizationPolicies();

builder.Services.AddScoped<JwtAuthenticatorProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());
builder.Services.AddScoped<ILoginServices, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());

// Inyección de Dependencias - Módulo de Send Email
builder.Services.AddScoped<ISendEmail, RSendEmail>();

// Inyección de Dependencias - Módulo de Catálogos
builder.Services.AddScoped<ICarrera, RCarrera>();
builder.Services.AddScoped<IEdificio, REdificio>();
builder.Services.AddScoped<IEscuela, REscuela>();
builder.Services.AddScoped<ILink, RLink>();
builder.Services.AddScoped<IPiso, RPiso>();
builder.Services.AddScoped<ITipoPersonal, RTipoPersonal>();
builder.Services.AddScoped<ITipoSolicitud, RTipoSolicitud>();
builder.Services.AddScoped<IAreaDepto, RAreaDepto>();
builder.Services.AddScoped<IExtension, RExtension>();

// Inyección de Dependencias - Módulo de Registro
builder.Services.AddScoped<IUsuario, RUsuario>();

builder.Services.AddScoped<SfDialogService>();
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
