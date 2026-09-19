using InsuranceApp.Auditing;
using InsuranceApp.DataAccess;
using InsuranceApp.Helper;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Testcontainers.MongoDb;
using Testcontainers.MsSql;

var builder = WebApplication.CreateBuilder(args);

// Start Testcontainers for SQL Server and MongoDB
var sqlContainer = (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
        ? new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        : new()

    ).Build();

var mongoContainer = new MongoDbBuilder()
    .WithImage("mongo:latest")
    .Build();

await sqlContainer.StartAsync();
await mongoContainer.StartAsync();

// Add services to the container.
builder.Services
    .AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddDbContext<AuditContext>(options =>
    options.UseSqlServer(
        sqlContainer.GetConnectionString(),
        sqlOptions => sqlOptions.MigrationsAssembly("InsuranceApp.Auditing")));

builder.Services.AddScoped<Auditer>();

var client = new MongoClient(mongoContainer.GetConnectionString());
//var database = client.GetDatabase(builder.Configuration["MongoDb:DBNAME"]); // Use a default/test database name
var database = client.GetDatabase("InsuranceAppDb");

builder.Services.AddDbContext<InsuranceAppDbContext>(options =>
{
    options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DependencyInjectionHelper.InjectRepository(builder.Services);
DependencyInjectionHelper.InjectServices(builder.Services);
DependencyInjectionHelper.InjectAuditing(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<InsuranceAppDbContext>();

    await DatabaseSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
    context.Database.Migrate();
}

app.Run();

public partial class Program { }
