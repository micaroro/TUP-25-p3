using Microsoft.EntityFrameworkCore;
using Servidor.Data;
using Servidor.Models;

namespace Servidor.Endpoints;

public static class ProductosEndpoints
{
    public static void MapProductos(this WebApplication app)
    {
        app.MapGet("/productos", async (TiendaContext db) =>
            await db.Productos.ToListAsync());

        app.MapGet("/productos/{id}", async (int id, TiendaContext db) =>
            await db.Productos.FindAsync(id) is Producto p ? Results.Ok(p) : Results.NotFound());

        app.MapPost("/compras", async (Compra compra, TiendaContext db) =>
        {
            compra.Fecha = DateTime.Now;
            db.Compras.Add(compra);
            await db.SaveChangesAsync();
            return Results.Ok(compra);
        });
    }
}
