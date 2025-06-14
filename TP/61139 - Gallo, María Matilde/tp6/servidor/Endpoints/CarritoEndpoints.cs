using Servidor.Data;
using Servidor.Models;

namespace Servidor.Endpoints;

public static class CarritoEndpoints
{
    public static void MapCarrito(this WebApplication app)
    {
        app.MapPost("/api/compra", async (Compra compra, TiendaContext db) =>
        {
            db.Compras.Add(compra);
            await db.SaveChangesAsync();
            return Results.Ok(compra.Id);
        });
    }
}
