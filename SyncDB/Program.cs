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
using SyncDB;

Worker worker = new Worker();

while(worker.IsRunning)
{
    await worker.RunAsync();
    //Console.WriteLine("ESPERANDO 1 MINUTO...");
    await Task.Delay(60000);
}
