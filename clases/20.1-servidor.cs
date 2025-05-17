#!/usr/bin/env dotnet script
#r "sdk:Microsoft.NET.Sdk.Web"

// EF Core 8 (especificamos versión explícita para evitar mezclar DLLs más antiguas)
#r "nuget: Microsoft.EntityFrameworkCore, 9.0.4"
#r "nuget: Microsoft.EntityFrameworkCore.Sqlite, 9.0.4"

using System.Text.Json;                     
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;            
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


// -----------------------------------------------------------------------------
//  CONFIGURACIÓN
// -----------------------------------------------------------------------------
var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<AppDb>(opt =>
    opt.UseSqlite("Data Source=agenda.db"));

// camelCase global para JSON
builder.Services.Configure<JsonOptions>(o =>
{
    o.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

var app = builder.Build();

// crear BD si no existe
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    db.Database.EnsureCreated();
}

// -----------------------------------------------------------------------------
//  END-POINTS
// -----------------------------------------------------------------------------
app.MapGet("/contactos", async (string q, AppDb db) =>
{
    var consulta = db.Contactos.AsQueryable();

    if (!string.IsNullOrWhiteSpace(q))
    {
        q = q.ToLowerInvariant();
        consulta = consulta.Where(c =>
            c.Nombre.ToLower().Contains(q)   ||
            c.Apellido.ToLower().Contains(q) ||
            c.Telefono.ToLower().Contains(q) ||
            c.Email.ToLower().Contains(q));
    }

    return await consulta
        .OrderBy(c => c.Apellido).ThenBy(c => c.Nombre)
        .ToListAsync();
});

app.MapGet("/contactos/{id:int}", async (int id, AppDb db) =>
    await db.Contactos.FindAsync(id));

app.MapPost("/contactos", async (Contacto nuevo, AppDb db) =>
{
    db.Contactos.Add(nuevo);
    await db.SaveChangesAsync();
    return Results.Created($"/contactos/{nuevo.Id}", nuevo);   // Results.* OK
});

app.MapPut("/contactos/{id:int}", async (int id, Contacto datos, AppDb db) =>
{
    var c = await db.Contactos.FindAsync(id);
    c.Nombre   = !string.IsNullOrWhiteSpace(datos.Nombre)   ? datos.Nombre   : c.Nombre;
    c.Apellido = !string.IsNullOrWhiteSpace(datos.Apellido) ? datos.Apellido : c.Apellido;
    c.Telefono = !string.IsNullOrWhiteSpace(datos.Telefono) ? datos.Telefono : c.Telefono;
    c.Email    = !string.IsNullOrWhiteSpace(datos.Email)    ? datos.Email    : c.Email;
    c.Edad     = datos.Edad != 0                            ? datos.Edad     : c.Edad;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/contactos/{id:int}", async (int id, AppDb db) =>
{
    var c = await db.Contactos.FindAsync(id);
    db.Contactos.Remove(c);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run("http://localhost:5000");

// -----------------------------------------------------------------------------
//  MODELOS
// -----------------------------------------------------------------------------
class Contacto
{
    public int    Id       { get; set; }
    public string Nombre   { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Email    { get; set; } = "";
    public int    Edad     { get; set; }
}

class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> opt) : base(opt) { }
    public DbSet<Contacto> Contactos => Set<Contacto>();
}