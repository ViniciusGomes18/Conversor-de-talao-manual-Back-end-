using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DSIN.Data.Contexts;

// DI de domínio/infrastructure
using DSIN.Business.Interfaces.IRepositories;
using DSIN.Business.Interfaces.IServices;
using DSIN.Business.Services;
using DSIN.Data.Repositories;
using DSIN.Data.External;

var builder = WebApplication.CreateBuilder(args);

// ---------- Config ----------
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// ---------- Services (MVC / Controllers) ----------
builder.Services.AddControllers();

// ---------- Banco de Dados ----------
var cs =
    builder.Configuration["DATABASE_URL"] ??
    builder.Configuration.GetConnectionString("Default") ??
    throw new InvalidOperationException("Não há connection string. Defina DATABASE_URL ou ConnectionStrings:Default.");

builder.Services.AddDbContext<TicketingDbContext>(options =>
{
    options.UseNpgsql(cs);
});

// ---------- Repositórios ----------
builder.Services.AddScoped<IAgentRepository, AgentRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ITicketBookRepository, TicketBookRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ---------- Serviços de Domínio / Aplicação ----------
builder.Services.AddScoped<IAgentService, AgentService>();
builder.Services.AddScoped<ITicketBookService, TicketBookService>();

// Cliente HTTP para IA (OpenAI OCR)
builder.Services.AddHttpClient<IOcrClient, OpenAiOcrClient>();

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------- Health Check simples ----------
app.MapGet("/healthz", async (TicketingDbContext db, CancellationToken ct) =>
{
    var canConnect = await db.Database.CanConnectAsync(ct);
    return canConnect
        ? Results.Ok("OK")
        : Results.StatusCode(503);
});

// ---------- Pipeline HTTP ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// (se quiser forçar HTTPS localmente; no Render não faz diferença)
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
