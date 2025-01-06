// See https://aka.ms/new-console-template for more information
using SyncDB.Common;
using System.Text.Json;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContextCentral;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SyncDB.Context;
using Microsoft.Data.SqlClient;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail;
using SyncDB;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using System.Text;



/* ================ CONFIGURACIÓN ================*/
var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

Worker worker = new Worker(config);

/*
// ENVIAR CORREO DE PRUEBA
RSendEmailService servicioCorreo = new RSendEmailService(config);

RequestDTO_SendEmail correo = new RequestDTO_SendEmail()
{
    Body = "CORREO DE PRUEBA CON ADJUNTOS",
    EmailTo = "gabrielmtzx7@gmail.com",
    Subject = "PRUEBA ADJUNTOS"
};

string archivo = "ARCHIVO TXT DE PRUEBA";

Dictionary<string, byte[]> adjuntos = new()
{
    {
        "archivo.txt",
        UTF8Encoding.UTF8.GetBytes(archivo)
    }
};

await servicioCorreo.SendEmailAsync(correo, adjuntos);
*/

while(worker.IsRunning)
{
    await worker.RunAsync();
    //Console.WriteLine("ESPERANDO 1 MINUTO...");
    await Task.Delay(60000);


}
