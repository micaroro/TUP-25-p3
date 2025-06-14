using Microsoft.EntityFrameworkCore;
using Servidor.Models;

namespace Servidor.Data
{
    public class TiendaContext : DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) { }

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Compra> Compras => Set<Compra>();
        public DbSet<ItemCompra> Items => Set<ItemCompra>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    Id = 1,
                    Nombre = "Zapatillas Adidas Gazelle Bold",
                    Descripcion = "Diseño rosa con verde. Plataforma triple y gamuza supersuave para un estilo clásico renovado.",
                    Precio = 15000,
                    Stock = 50,
                    ImagenUrl = "img/zapa1.jpg"
                },
                new Producto
                {
                    Id = 2,
                    Nombre = "Zapatillas Adidas London",
                    Descripcion = "Verde y rosa vibrantes.  Las adidas London Green Pink están inspiradas en la colaboración Gucci x Gazelle.",
                    Precio = 250000,
                    Stock = 30,
                    ImagenUrl = "img/zapa2.jpg"
                },
                new Producto
                {
                    Id = 3,
                    Nombre = "Zapatillas Puma Palermo",
                    Descripcion = "Modelo clásico de los 80. Rosa vibrante con esencia futbolera británica.",
                    Precio = 185000,
                    Stock = 30,
                    ImagenUrl = "img/zapa3.jpg"
                }
            );
        }
    }
}
