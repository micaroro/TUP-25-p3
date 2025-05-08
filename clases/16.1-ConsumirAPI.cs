using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


static async Task ObtenerClima() {
    Write("Ingrese una ciudad: ");
    string ciudad = ReadLine()?.Trim() ?? "Tucuman";

    string apiKey = "30d38b26954359266708f92e1317dac0";

    string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={apiKey}&units=metric&lang=es";

    WriteLine($"URL de la API: {url}");
    using var http = new HttpClient();
    var response = await http.GetStringAsync(url);
    WriteLine(response);
    var data = JsonDocument.Parse(response);

    var temp  = data.RootElement.GetProperty("main").GetProperty("temp").GetDouble();
    var clima = data.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();

    WriteLine(url);
    WriteLine($"""
    
    === Clima Actual ===    
        
        Ciudad     : {ciudad}
        Temperatura: {temp} °C
        Condición  : {clima}

""");
}
await ObtenerClima();