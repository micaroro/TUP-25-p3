using System;                     // Console, etc.
using System.Linq;                // OrderBy
using System.Net.Http;            // HttpClient, StringContent ✔
using System.Text.Json;           // JsonSerializer, JsonNamingPolicy
using System.Threading.Tasks;     // Task

var baseUrl = "http://localhost:5000";

var http    = new HttpClient();
var jsonOpt = new JsonSerializerOptions {
    PropertyNamingPolicy        = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true
};

async Task<List<Contacto>> ObtenerAsync(string q = null) {
    var url = string.IsNullOrWhiteSpace(q)
        ? $"{baseUrl}/contactos"
        : $"{baseUrl}/contactos?q={Uri.EscapeDataString(q)}";
    var json = await http.GetStringAsync(url);
    return JsonSerializer.Deserialize<List<Contacto>>(json, jsonOpt)!;
}

Contacto PedirDatos() {
    Console.Write("Nombre: ");   var nombre   = Console.ReadLine()!;
    Console.Write("Apellido: "); var apellido = Console.ReadLine()!;
    Console.Write("Teléfono: "); var telefono = Console.ReadLine()!;
    Console.Write("Email: ");    var email    = Console.ReadLine()!;
    Console.Write("Edad: ");     var edad     = int.Parse(Console.ReadLine()!);
    return new Contacto { Nombre = nombre, Apellido = apellido, Telefono = telefono, Email = email, Edad = edad };
}

async Task MostrarAsync(IEnumerable<Contacto> lista) {
    // Encabezados de columna
    Console.WriteLine(
        $"{"ID",-4} {"Apellido",-15} {"Nombre",-15} {"Teléfono",-15} {"Email",-25} {"Edad",-4}"
    );
    Console.WriteLine(new string('-', 80));
    foreach (var c in lista.OrderBy(c => c.Apellido).ThenBy(c => c.Nombre)){
        Console.WriteLine(
        $"{c.Id,-4} {c.Apellido,-15} {c.Nombre,-15} {c.Telefono,-15} {c.Email,-25} {c.Edad,-4}"
        );
    }
}

while (true) {
    Console.WriteLine("\nMenú: \n 1 Agregar \n 2 Borrar \n 3 Editar \n 4 Listar \n 5 Buscar \n 0 Salir");
    var op = Console.ReadLine();
    if (op == "0") break;

    switch (op)
    {
        case "1":
            var nuevo = PedirDatos();
            await http.PostAsync($"{baseUrl}/contactos",
                new StringContent(JsonSerializer.Serialize(nuevo, jsonOpt), System.Text.Encoding.UTF8, "application/json"));
            Console.WriteLine("Contacto agregado.");
            break;

        case "2":
            Console.Write("Id a borrar: ");
            var idBorrar = Console.ReadLine();
            await http.DeleteAsync($"{baseUrl}/contactos/{idBorrar}");
            Console.WriteLine("Contacto borrado.");
            break;

        case "3":
            Console.Write("Id a editar: ");
            var idEdit = Console.ReadLine();
            var datos  = PedirDatos();
            await http.PutAsync($"{baseUrl}/contactos/{idEdit}",
                new StringContent(JsonSerializer.Serialize(datos, jsonOpt), System.Text.Encoding.UTF8, "application/json"));
            Console.WriteLine("Contacto actualizado.");
            break;

        case "4":
            await MostrarAsync(await ObtenerAsync());
            break;

        case "5":
            Console.Write("Texto a buscar: ");
            var texto = Console.ReadLine();
            await MostrarAsync(await ObtenerAsync(texto));
            break;
    }
}

class Contacto {
    public int    Id       { get; set; }
    public string Nombre   { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Email    { get; set; }
    public int    Edad     { get; set; }
}
