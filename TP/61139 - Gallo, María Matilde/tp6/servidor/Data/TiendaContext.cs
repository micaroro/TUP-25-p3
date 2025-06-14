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
public DbSet<Carrito> Carritos => Set<Carrito>();
public DbSet<ItemCarrito> ItemsCarrito => Set<ItemCarrito>();


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
                    Descripcion = "Verde y rosa vibrantes. Las adidas London Green Pink están inspiradas en la colaboración Gucci x Gazelle.",
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
                },
                new Producto
                {
                    Id = 4,
                    Nombre = "Zapatillas Nike Air Max",
                    Descripcion = "Comodidad y estilo para correr con tecnología Air Max.",
                    Precio = 20000,
                    Stock = 40,
                    ImagenUrl = "img/zapa4.jpg"
                },
                new Producto
                {
                    Id = 5,
                    Nombre = "Zapatilla Dc",
                    Descripcion = "Diseño retro con comodidad moderna.",
                    Precio = 180000,
                    Stock = 35,
                    ImagenUrl = "img/zapa5.jpg"
                },
                new Producto
                {
                    Id = 6,
                    Nombre = "Zapatillas New Balance 574",
                    Descripcion = "Estilo clásico y comodidad todo el día.",
                    Precio = 220000,
                    Stock = 45,
                    ImagenUrl = "img/zapa6.jpg"
                },
                new Producto
                {
                    Id = 7,
                    Nombre = "Zapatillas Nike Dunk Low",
                    Descripcion = "Perfectas para correr largas distancias con soporte extra.",
                    Precio = 270000,
                    Stock = 25,
                    ImagenUrl = "img/zapa7.jpg"
                },
                new Producto
                {
                    Id = 8,
                    Nombre = "Zapatillas Converse Chuck Taylor",
                    Descripcion = "Clásicas y atemporales, con estilo urbano.",
                    Precio = 140000,
                    Stock = 50,
                    ImagenUrl = "img/zapa8.jpg"
                },
                new Producto
                {
                    Id = 9,
                    Nombre = "Zapatillas Vans Old Skool",
                    Descripcion = "Diseño icónico para uso casual y skateboarding.",
                    Precio = 160000,
                    Stock = 40,
                    ImagenUrl = "img/zapa9.jpg"
                },
                new Producto
                {
                    Id = 10,
                    Nombre = "Zapatillas Saucony Jazz",
                    Descripcion = "Comodidad y estilo para uso diario y entrenamiento.",
                    Precio = 190000,
                    Stock = 30,
                    ImagenUrl = "img/zapa10.jpg"
                }
            );
        }
    }
}
