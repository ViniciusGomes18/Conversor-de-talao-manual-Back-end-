using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DSIN.Business.Models;
using DSIN.Data.Contexts;

var builder = WebApplication.CreateBuilder(args);

// ---------- Config ----------
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// ---------- Services ----------
builder.Services.AddControllers();

// String de conexão: primeiro DATABASE_URL (Render), depois ConnectionStrings:Default
var cs = builder.Configuration.GetConnectionString("Default")
    ?? throw new Exception("Connection string não encontrada");

builder.Services.AddDbContext<TicketingDbContext>(options =>
    options.UseNpgsql(cs)
);

// DbContext apontando para o assembly de migrations DSIN
builder.Services.AddDbContext<TicketingDbContext>(options =>
    options.UseNpgsql(
        cs,
        b => b.MigrationsAssembly("DSIN")
    )
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ---------- Migrations + Seed de usuário de teste ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketingDbContext>();

    // Cria o banco / aplica todas as migrations pendentes
    db.Database.Migrate();

    // Seed: cria 1 agente de teste se ainda não existir
    if (!db.Agents.Any())
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("123456");

        var seedAgent = new Agent(
            Guid.NewGuid(),
            "Agente Teste",
            "agente@dsin.com",
            hash
        );

        db.Agents.Add(seedAgent);
        db.SaveChanges();
    }
}

// ---------- Endpoint de healthcheck ----------
app.MapGet("/healthz", async (TicketingDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
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

app.MapControllers();

app.Run();
