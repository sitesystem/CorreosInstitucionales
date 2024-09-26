global using CorreosInstitucionales.Server.CapaDataAccess.DBContext;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.WebEncoders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

using Serilog;

using CorreosInstitucionales.Server.CapaDataAccess.Controllers.LoginAuth;
using CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail;
using CorreosInstitucionales.Shared.CapaEntities.Common;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Customize Encoding Settings
builder.Services.AddSingleton(HtmlEncoder.Create(allowedRanges: [ UnicodeRanges.BasicLatin, UnicodeRanges.LatinExtendedA ]));
builder.Services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

// Inyecci�n de Dependencias
//public void ConfigureServices(IServiceCollection services)
//{
//    services.Configure<reCAPTCHAVerificationOptions>(Configuration.GetSection("reCAPTCHA"));
//    services.AddHttpClient();
//    services.AddMvc();
//}

// builder.Services.Configure<reCAPTCHAVerificationOptions>(Configuration.GetSection("reCAPTCHA"));

/* IMPORTANTE: AGREGAR LA OPCI�N DE CONFIGURACI�N DONDE SE ESTABLECE LA VERSI�N DE SQL SERVER (120 -> 12.0); DE LO CONTRARIO, SE ROMPEN ALGUNAS INSTRUCCIONES DEL LINQ */
builder.Services.AddDbContext<DbCorreosInstitucionalesUpiicsaContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer_Connection"),
    ob => ob.UseCompatibilityLevel(120));
});

// LogReg (Archivo de Registros de Eventos)
// Log.Logger = new LoggerConfiguration().MinimumLevel.Verbose()
//    .WriteTo.Console()
//    .WriteTo.File("wwwroot/repositorio/Logs/log-.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss}")
//    .CreateLogger();
// Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog((hostingContext, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
});
// builder.Logging.AddConsole();

// Habilitar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // T�tulo Dise�o
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "serverWS", Version = "v1" });

    // Bot�n Authorize
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Servicios CORS
string? cors = "_cors";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: cors,
        builder =>
        {
            builder.WithHeaders("*"); // POST
            builder.WithOrigins("*").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); // GET
            builder.WithMethods("*"); // PUT DELETE
        });
});

builder.Services.AddAuthorization(options =>
{
    // options.AddPolicy("[Rol] Developer", policy => policy.RequireClaim("Rol", "1", "2"));
    // options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
});

// JWT (Jason Web Token)
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.UTF8.GetBytes(appSettings?.Secreto ?? string.Empty); // var key = Encoding.ASCII.GetBytes(appSettings.Secreto);

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false, // Emisor
        ValidateAudience = false, // Resource Server

        // ValidAudience = builder.Configuration["AuthSettings:Audince"],
        // ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
        // RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});

// Dependency Injection (Inyectar e Implementar la Interfaz)
builder.Services.AddScoped<ILoginAuthService, RLoginAuthService>();

// Inyecci�n de Dependencias - M�dulo de Send Email & Send WhatsApp
builder.Services.AddScoped<ISendEmailService, RSendEmailService>();
builder.Services.AddScoped<ISendWhatsAppService, RSendWhatsAppService>();

// HTTCLIENT QUE ACEPTA HTTPS CON CERTIFICADOS AUTOFIRMADOS
builder.Services.AddScoped
(
    sp => new HttpClient
    (
        new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, cert, cetChain, policyErrors) =>
                            {
                                return true;
                            }
        }
    )
);

builder.Services.AddMemoryCache();

/******************************************************************************************************/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "serverWS v1"));
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(cors);

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

app.UseRouting();

// Middlewares para gestionar la Authentication y la Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
