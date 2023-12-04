using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

using CorreosInstitucionales.Server.CapaDataAccess.LoginAuth;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Inyección de Dependencias
//public void ConfigureServices(IServiceCollection services)
//{
//    services.Configure<reCAPTCHAVerificationOptions>(Configuration.GetSection("reCAPTCHA"));
//    services.AddHttpClient();
//    services.AddMvc();
//}

//builder.Services.Configure<reCAPTCHAVerificationOptions>(Configuration.GetSection("reCAPTCHA"));

// Habilitar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Título Diseño
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "serverWS", Version = "v1" });

    // Botón Authorize
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
    options.AddPolicy("[Rol] Developer", policy => policy.RequireClaim("Rol", "1", "2"));
    //options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Manager"));
});

// JWT (Jason Web Token)
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

var appSettings = appSettingsSection.Get<AppSettings>();
//var key = Encoding.ASCII.GetBytes(appSettings.Secreto);
var key = Encoding.UTF8.GetBytes(appSettings.Secreto);

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

        //ValidAudience = builder.Configuration["AuthSettings:Audince"],
        //ValidIssuer = builder.Configuration["AuthSettings:Issuer"],
        //RequireExpirationTime = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});

// Dependency Injection (Inyectar e Implementar la Interfaz)
builder.Services.AddScoped<ILoginService, RLoginService>();

/******************************************************************************************************/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    //app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "serverWS v1"));

app.UseCors(cors);

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

// Middlewares para gestionar la Authentication y la Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
