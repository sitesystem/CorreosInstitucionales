using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Radzen;

using CorreosInstitucionales.Client;
using CorreosInstitucionales.Client.CapaPresentationComponentsPagesUI_UX.Login;
using CorreosInstitucionales.Client.Shared.Components.Utils;

using CorreosInstitucionales.Shared.CapaServices.BusinessLogic;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolDebug;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Inyecci�n de Dependencias - Portapapeles
builder.Services.AddScoped<IClipboardService, ClipboardService>();

// Inyecci�n de Dependencias - M�dulo de Login
builder.Services.AddAuthorizationCore();
builder.Services.ConfigureAuthorizationPolicies();

builder.Services.AddScoped<JwtAuthenticatorProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());
builder.Services.AddScoped<ILoginServices, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());

// Inyecci�n de Dependencias - M�dulo de Archivos
builder.Services.AddScoped<RArchivosService>();

// Inyecci�n de Dependencias - M�dulo de Send Email & Send WhatsApp
builder.Services.AddScoped<ISendEmailService, RSendEmailService>();
builder.Services.AddScoped<ISendWhatsAppService, RSendWhatsAppService>();

// Inyecci�n de Dependencias - M�dulo de Registro del Usuario
builder.Services.AddScoped<RUsuarioService>();

// Inyecci�n de Dependencias - M�dulo de Solicitudes-Tickets
builder.Services.AddScoped<RSolicitudService>();

// Inyecci�n de Dependencias - M�dulo de Cat�logos
builder.Services.AddScoped<RAreaDeptoService>();
builder.Services.AddScoped<RCarreraService>();
builder.Services.AddScoped<REdificioService>();
builder.Services.AddScoped<REscuelaService>();
builder.Services.AddScoped<REstadosSolicitudService>();
builder.Services.AddScoped<RLinkService>();
builder.Services.AddScoped<RNoExtensionService>();
builder.Services.AddScoped<RRolesService>();
builder.Services.AddScoped<RPisoService>();
builder.Services.AddScoped<RTipoPersonalService>();
builder.Services.AddScoped<RTipoSolicitudService>();
builder.Services.AddScoped<RAnuncioService>();

// Inyecci�n de Dependencias - M�dulo de Estad�sticas
builder.Services.AddScoped<REstadisticasService>();

// Inyecci�n de Dependencias - M�dulo de Pruebas
builder.Services.AddScoped<RDebugService>();

// Radzen Components and Services
builder.Services.AddRadzenComponents();
// builder.Services.AddScoped<ContextMenuService>();
// builder.Services.AddScoped<DialogService>();
// builder.Services.AddScoped<NotificationService>();
// builder.Services.AddScoped<TooltipService>();

builder.Services.AddLogging();

await builder.Build().RunAsync();
