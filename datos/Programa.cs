// crudcrud.com rechaza payloads cuyos keys comienzan con mayúscula, por eso la serialización en camelCase
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Console;

public class ContactoRequest {
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("telefono")]
    public string Telefono { get; set; }
}

public class ContactoResponse {
    public string _id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
}

public class CrudContactos {
    private static readonly HttpClient client = new HttpClient();
    private static readonly string baseUrl = "https://crudcrud.com/api/your-api-key/contactos";
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };

    private static StringContent ToJson<T>(T objeto) {
        var options = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,          // claves en camelCase
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        var json = JsonSerializer.Serialize(objeto, options);
        return new StringContent(json, new UTF8Encoding(false), "application/json");
    }

    public static async Task<List<ContactoResponse>> GetAll() {
        var response = await client.GetAsync(baseUrl);
        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            WriteLine($"⚠️  Error {response.StatusCode}: {error}");
        }
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ContactoResponse>>(json, jsonOptions);
    }

    public static async Task<ContactoResponse> Get(string id) {
        var response = await client.GetAsync($"{baseUrl}/{id}");
        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            WriteLine($"⚠️  Error {response.StatusCode}: {error}");
        }
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContactoResponse>(json, jsonOptions);
    }

    public static async Task<ContactoResponse> Create(ContactoRequest contacto) {
        WriteLine($"POST Body: {JsonSerializer.Serialize(contacto, jsonOptions)}");
        var response = await client.PostAsync(baseUrl, ToJson(contacto));
        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            WriteLine($"⚠️  Error {response.StatusCode}: {error}");
        }
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContactoResponse>(json, jsonOptions);
    }

    public static async Task<ContactoResponse> Update(string id, ContactoRequest contacto) {
        var response = await client.PutAsync($"{baseUrl}/{id}", ToJson(contacto));
        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            WriteLine($"⚠️  Error {response.StatusCode}: {error}");
        }
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ContactoResponse>(json, jsonOptions);
    }

    public static async Task Delete(string id) {
        var response = await client.DeleteAsync($"{baseUrl}/{id}");
        if (!response.IsSuccessStatusCode) {
            var error = await response.Content.ReadAsStringAsync();
            WriteLine($"⚠️  Error {response.StatusCode}: {error}");
        }
        response.EnsureSuccessStatusCode();
    }
}