using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor;
using System.Globalization;

using CorreosInstitucionales.Client;
using CorreosInstitucionales.Client.Shared;
using CorreosInstitucionales.Client.CapaPresentation.ComponentsPages.UI_UX.Login;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catCarreras;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEdificios;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEscuelas;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catLinks;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catPisos;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposPersonal;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposSolicitud;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.tbUsuarios;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catAreasDeptos;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catNoExtensiones;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail;

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
builder.Services.AddScoped<ISendEmailService, RSendEmailService>();

// Inyección de Dependencias - Módulo de Registro
builder.Services.AddScoped<IUsuarioService, RUsuarioService>();

// Inyección de Dependencias - Módulo de Catálogos
builder.Services.AddScoped<IAreaDeptoService, RAreaDeptoService>();
builder.Services.AddScoped<ICarreraService, RCarreraService>();
builder.Services.AddScoped<IEdificioService, REdificioService>();
builder.Services.AddScoped<IEscuelaService, REscuelaService>();
builder.Services.AddScoped<ILinkService, RLinkService>();
builder.Services.AddScoped<INoExtensionService, RNoExtensionService>();
builder.Services.AddScoped<IPisoService, RPisoService>();
builder.Services.AddScoped<ITipoPersonalService, RTipoPersonalService>();
builder.Services.AddScoped<ITipoSolicitudService, RTipoSolicitudService>();

// Radzen Components and Services
builder.Services.AddRadzenComponents();
//builder.Services.AddScoped<ContextMenuService>();
//builder.Services.AddScoped<DialogService>();
//builder.Services.AddScoped<NotificationService>();
//builder.Services.AddScoped<TooltipService>();

// Syncfusion Components and Services
builder.Services.AddSyncfusionBlazor(options => { options.EnableRtl = false; options.Animation = GlobalAnimationMode.Enable; /*options.IgnoreScriptIsolation = true;*/ });
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjk5MDM2M0AzMjMzMmUzMDJlMzBJSEhqU1Yra0NaTkZwZDIzWTZ6MldNSkR4d3VkRWhMZmpJc1dUVTFOU2VBPQ=="); // Trial Developer

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
