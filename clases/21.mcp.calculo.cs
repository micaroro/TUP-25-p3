#!/usr/bin/env dotnet script
#r "sdk:Microsoft.NET.Sdk.Web"
#r "nuget: ModelContextProtocol, 0.2.0-preview.1"

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Server;

var builder = Host.CreateEmptyApplicationBuilder(settings: null);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();

[McpServerToolType]
public static class CalculoTool {
    [McpServerTool, Description("Devuelve el mensaje al cliente.")]
    public static string Eco(string message) => $"Eco: {message}";

    [McpServerTool, Description("Devuelve el texto invertido al cliente.")]
    public static string InvertirTexto(string texto) {
        var reversed = new char[texto.Length];
        for (int i = 0; i < texto.Length; i++)
            reversed[i] = texto[texto.Length - 1 - i];
        return $": {new string(reversed)} :";
    }

    [McpServerTool, Description("Calcula el factorial de un número entero no negativo.")]
    public static long Factorial(int numero) {
        if (numero < 0)
            throw new ArgumentException("El número debe ser no negativo.", nameof(numero));
        long resultado = 1;
        for (int i = 2; i <= numero; i++)
            resultado *= i;
        return resultado;
    }

    [McpServerTool, Description("Devuelve una lista con los n primeros números primos.")]
    public static List<int> PrimerosPrimos(int n) {
        if (n <= 0)
            throw new ArgumentException("El valor debe ser mayor que cero.", nameof(n));
        var primos = new List<int>();
        int candidato = 2;
        while (primos.Count < n) {
            if (EsPrimo(candidato))
                primos.Add(candidato);
            candidato++;
        }
        return primos;
    }

    private static bool EsPrimo(int numero) {
        if (numero < 2) return false;
        if (numero == 2) return true;
        if (numero % 2 == 0) return false;
        int limite = (int)Math.Sqrt(numero);
        for (int i = 3; i <= limite; i += 2)
            if (numero % i == 0)
                return false;
        return true;
    }

    [McpServerTool, Description("Devuelve la fecha y hora actual del servidor.")]
    public static string FechaHoraActual() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
}
