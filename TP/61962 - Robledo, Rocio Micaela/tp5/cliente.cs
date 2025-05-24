#r "nuget: System.Net.Http.Json"

using System.Net.Http;
using System.Net.Http.Json;

var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

while (true)
{
    Console.WriteLine("\n--- Menú ---");
    Console.WriteLine("1. Listar productos");
    Console.WriteLine("2. Listar productos a reponer");
    Console.WriteLine("3. Agregar stock");
    Console.WriteLine("4. Quitar stock");
    Console.WriteLine("5. Salir");
    Console.Write("Seleccione una opción: ");
    var opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            var productos = await client.GetFromJsonAsync<List<Producto>>("/productos");
            productos.ForEach(p => Console.WriteLine($"{p.Id}: {p.Nombre} - Stock: {p.Stock}"));
            break;

        case "2":
            var reponer = await client.GetFromJsonAsync<List<Producto>>("/productos/reponer");
            reponer.ForEach(p => Console.WriteLine($"{p.Id}: {p.Nombre} - Stock: {p.Stock}"));
            break;

        case "3":
            Console.Write("ID del producto: ");
            int idAgregar = int.Parse(Console.ReadLine());
            Console.Write("Cantidad a agregar: ");
            int cantAgregar = int.Parse(Console.ReadLine());
            await client.PostAsync($"/productos/agregar?id={idAgregar}&cantidad={cantAgregar}", null);
            break;

        case "4":
            Console.Write("ID del producto: ");
            int idQuitar = int.Parse(Console.ReadLine());
            Console.Write("Cantidad a quitar: ");
            int cantQuitar = int.Parse(Console.ReadLine());
            var quitarResp = await client.PostAsync($"/productos/quitar?id={idQuitar}&cantidad={cantQuitar}", null);
            if (!quitarResp.IsSuccessStatusCode)
                Console.WriteLine("Error: Stock insuficiente o producto no encontrado.");
            break;

        case "5":
            return;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
}

record Producto(int Id, string Nombre, int Stock);
