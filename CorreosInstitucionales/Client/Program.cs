using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;
using System.Globalization;

using CorreosInstitucionales.Client;
using CorreosInstitucionales.Client.CapaPresentation.ComponentsPages.UI_UX.Login;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail;
using CorreosInstitucionales.Client.Shared.Components.Utils;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Inyección de Dependencias - Portapapeles
builder.Services.AddScoped<IClipboardService, ClipboardService>();

// Inyección de Dependencias - Módulo de Login
builder.Services.AddAuthorizationCore();
builder.Services.ConfigureAuthorizationPolicies();

builder.Services.AddScoped<JwtAuthenticatorProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());
builder.Services.AddScoped<ILoginServices, JwtAuthenticatorProvider>(provider => provider.GetRequiredService<JwtAuthenticatorProvider>());

// Inyección de Dependencias - Módulo de Archivos
builder.Services.AddScoped<RArchivosService>();

// Inyección de Dependencias - Módulo de Send Email
builder.Services.AddScoped<ISendEmailService, RSendEmailService>();

// Inyección de Dependencias - Módulo de Registro del Usuario
builder.Services.AddScoped<RUsuarioService>();

// Inyección de Dependencias - Módulo de Solicitudes-Tickets
builder.Services.AddScoped<RSolicitudService>();

// Inyección de Dependencias - Módulo de Catálogos
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

// Inyección de Dependencias - Módulo de Estadísticas
builder.Services.AddScoped<REstadisticasService>();

// Radzen Components and Services
builder.Services.AddRadzenComponents();
// builder.Services.AddScoped<ContextMenuService>();
// builder.Services.AddScoped<DialogService>();
// builder.Services.AddScoped<NotificationService>();
// builder.Services.AddScoped<TooltipService>();

await builder.Build().RunAsync();
