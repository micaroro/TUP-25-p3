#r "nuget: Microsoft.EntityFrameworkCore.Sqlite"
#r "nuget: Microsoft.EntityFrameworkCore.InMemory"
#r "nuget: Microsoft.AspNetCore.App"

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder();
builder.Services.AddDbContext<TiendaDb>(opt => opt.UseSqlite("Data Source=tienda.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TiendaDb>();
    db.Database.EnsureCreated();
    if (!db.Productos.Any())
    {
        for (int i = 1; i <= 10; i++)
        {
            db.Productos.Add(new Producto { Nombre = $"Producto {i}", Stock = 10 });
        }
        db.SaveChanges();
    }
}

app.MapGet("/productos", async (TiendaDb db) => await db.Productos.ToListAsync());

app.MapGet("/productos/reponer", async (TiendaDb db) =>
    await db.Productos.Where(p => p.Stock < 3).ToListAsync());

app.MapPost("/productos/agregar", async (TiendaDb db, int id, int cantidad) =>
{
    var producto = await db.Productos.FindAsync(id);
    if (producto == null) return Results.NotFound();
    producto.Stock += cantidad;
    await db.SaveChangesAsync();
    return Results.Ok(producto);
});

app.MapPost("/productos/quitar", async (TiendaDb db, int id, int cantidad) =>
{
    var producto = await db.Productos.FindAsync(id);
    if (producto == null) return Results.NotFound();
    if (producto.Stock < cantidad) return Results.BadRequest("Stock insuficiente.");
    producto.Stock -= cantidad;
    await db.SaveChangesAsync();
    return Results.Ok(producto);
});

app.Run();

class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Stock { get; set; }
}

class TiendaDb : DbContext
{
    public TiendaDb(DbContextOptions<TiendaDb> options) : base(options) { }
    public DbSet<Producto> Productos { get; set; }
}

