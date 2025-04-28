//
// Ejemplo que ilustra el principio de responsabilidad única (SRP)
// 

using System;
using System.Collections.Generic;

class Producto {
    public int Id { get; set; }
    public string Nombre { get; set; }

    public Producto(int id, string nombre) {
        Id = id;
        Nombre = nombre;
    }
}

class ProductoPerecedero : Producto {
    public DateTime FechaVencimiento { get; set; }

    public ProductoPerecedero(int id, string nombre, DateTime fechaVencimiento) : base(id, nombre) {
        FechaVencimiento = fechaVencimiento;
    }
}

// Las funciones se trasladan a clases separadas
class Notificar {
    public void Info(string mensaje) {
        Console.WriteLine(mensaje);
    }
}
class Repositorio {
    private Dictionary<int, int> stock = new Dictionary<int, int>();

    public int ConsultarStock(Producto producto) {
        return stock.TryGetValue(producto.Id, out int cantidad) ? cantidad : 0;
    }

    public void AjustarStock(Producto producto, int cantidad) {
        var actual = ConsultarStock(producto);
        var nuevo = actual + cantidad;
        stock[producto.Id] = nuevo;
    }
}

class Politica {
    int Limite { get; set; }
    public Politica(int limite) {
        Limite = limite;
    }
    public bool EsNecesarioReabastecer(int cantidad) {
        return cantidad < Limite;
    }
    public bool EsStockNegativo(int cantidad) {
        return cantidad < 0;
    }
    public bool EsCercanoVencimiento(ProductoPerecedero producto) {
        return producto.FechaVencimiento < DateTime.Now.AddDays(3);
    }
}

class Programa {

    public static void Main(){
        Notificar notificar = new Notificar();
        Repositorio repositorio = new Repositorio();
        Politica politica = new Politica(5);

        var stock = new Dictionary<int, int>();

        int ConsultarStock(Producto producto) {
            return stock.TryGetValue(producto.Id, out int cantidad) ? cantidad : 0;
        }

        void AjustarStock(Producto producto, int cantidad) {
            var actual = repositorio.ConsultarStock(producto);
            var nuevo = actual + cantidad;
            repositorio.AjustarStock(producto, nuevo);

            if (politica.EsNecesarioReabastecer( nuevo )) {
                notificar.Info($"Es necesario reabastecer el producto {producto.Nombre}: ({nuevo} < 5)");
            }

            if (producto is ProductoPerecedero pp) {
                if(politica.EsCercanoVencimiento(pp)) {
                    notificar.Info($"El producto {producto.Nombre} está por vencer: ({pp.FechaVencimiento})");
                }
            }

            if (politica.EsStockNegativo(nuevo)) {
                notificar.Info($"El producto {producto.Nombre} no puede tener stock negativo: ({nuevo})");
            }
        }

        var reloj = new Producto(1, "Reloj");
        var manzana = new ProductoPerecedero(2, "Manzana", DateTime.Now.AddDays(5));

        // Stock inicial
        AjustarStock(reloj, 10);
        AjustarStock(manzana, 10);

        Console.WriteLine($"Stock {reloj.Nombre} antes: {ConsultarStock(reloj)}");
        AjustarStock(reloj, -5);
        Console.WriteLine($"Stock {reloj.Nombre} después: {ConsultarStock(reloj)}");

        Console.WriteLine($"Stock {manzana.Nombre} antes: {ConsultarStock(manzana)}");
        AjustarStock(manzana, -8);
        Console.WriteLine($"Stock {manzana.Nombre} después: {ConsultarStock(manzana)}");

    }
}

Programa.Main();