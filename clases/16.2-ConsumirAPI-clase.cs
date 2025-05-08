using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class ClimaInfo {
    public MainInfo Main { get; set; }
    public WeatherInfo[] Weather { get; set; }

    public class MainInfo {
        public double Temp { get; set; }
    }

    public class WeatherInfo {
        public string Description { get; set; }
    }
}

static async Task ObtenerClima() {
    Console.Write("Ingrese una ciudad: ");
    string ciudad = Console.ReadLine()?.Trim() ?? "Tucuman";
    string apiKey = "30d38b26954359266708f92e1317dac0";
    string url = $"https://api.openweathermap.org/data/2.5/weather?q={ciudad}&appid={apiKey}&units=metric&lang=es";

    using var http = new HttpClient();
    var response = await http.GetStringAsync(url);

    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var data = JsonSerializer.Deserialize<ClimaInfo>(response, options);

    Console.WriteLine($"""
        === Clima Actual ===    
        Ciudad     : {ciudad}
        Temperatura: {data?.Main?.Temp} °C
        Condición  : {data?.Weather?[0]?.Description}
    """);
}

await ObtenerClima();