#r "sdk:Microsoft.NET.Sdk.Web"

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


// La ruta esta definida por el metodos HTTP y la URL
// En este caso la ruta es la misma para todos los metodos HTTP
app.MapGet("probar/",    () => "GET desde /probar");
app.MapPost("/probar",   () => "POST desde /probar");
app.MapPut("/probar",    () => "PUT desde /probar");
app.MapDelete("/probar", () => "DELETE desde /probar");


// Se le puede pasar parametros en el query string
// (la variables se reciben como argumentos)
// GET /buscar?q=texto
app.MapGet("/buscar", (string q) =>
    $"El texto a buscar es '{q}' (Usando query string)"
);

// O por route parameters
// Las variable se recibe como argumento pero tambien debe definirse en la ruta
// GET /buscar/texto
app.MapGet("/buscar/{texto}", (string texto) =>
    $"El texto a buscar es '{texto}' (Usando route parameter)"
);

// O combinando ambos
// GET /traer/texto?page=2
app.MapGet("/traer/{id}", (string id, int pagina) =>
    $"""
    El id a buscar es '{id}' (Usando route parameter) 
    de la pagina {pagina} (usando query string)
    """
);


// Un ejemplo mas complejo (Especificidad de las rutas)
// La ruta es estatica
app.MapGet("/noticias", () =>
    "Lista de noticias"
);

// La ruta es dinamica (noticias es estatica {seccion} es dinamica)
app.MapGet("/noticias/{seccion}", (string seccion) =>
    $"Noticias de la sección {seccion}"
);

// La ruta es dinamica (noticias es estatica {año} es dinamica {mes} es dinamica {dia} es dinamica)
app.MapGet("/noticias/{año}/{mes}/{dia}", (int año, int mes, int dia, int pagina) => 
    $"La fecha es {new DateTime(año, mes, dia):dd/MM/yyyy} en la pagina {pagina}"
);



// Devuelvo un json en forma manual (Observar que se usa `$$` y luego `{{ }}` para interpolar)
app.MapGet("/persona/{nombre}/{edad}", (string nombre, int edad) => 
    $$"""
    {
        "Nombre": "{{nombre}}",
        "Edad": {{edad}}
    }
    """
);

// Devolver un json de forma automatica (Cuando se devuelve un objeto convierte a json automaticamente)
app.MapGet("/persona", () => {
    var id = Guid.NewGuid();
    var persona = new{
        Id = id,
        Nombre = "Juan",
        Edad = 30
    };
    return Results.Accepted($"/persona/{id}", persona);
});


// Retornando un json usando una clase anonima
app.MapGet("/persona/{nombre}/{apellido}/{telefono}", (string nombre, string apellido, string telefono) =>
    Results.Ok(new {nombre, apellido, telefono})
);


app.Run();

