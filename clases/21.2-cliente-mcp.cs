#!/usr/bin/env dotnet script
#r "sdk:Microsoft.NET.Sdk.Web"                  // top-level await  [oai_citation:13‡Stack Overflow](https://stackoverflow.com/questions/65426934/does-dotnet-script-csx-support-asp-net-core-app-development?utm_source=chatgpt.com)
#r "nuget: ModelContextProtocol, 0.2.0-preview.1"       // (o la versión que uses)

using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using System.Linq;

// Lanza el servidor MCP definido en 21.servidor-mcp.csx mediante dotnet-script.
var clientTransport = new StdioClientTransport(
    new StdioClientTransportOptions {
        Name = "MiMCP",
        Command = "dotnet",
        Arguments = new[] { "script", "./21.1-servidor-mcp.cs" }
    });

var cliente = await McpClientFactory.CreateAsync(clientTransport);

Console.WriteLine("=== Herramientas disponibles ===");
foreach (var tool in await cliente.ListToolsAsync()) {
    Console.WriteLine($"- {tool.Name} ({tool.Description})");
}

async Task<long> Factorial(int numero) {
    var result = await cliente.CallToolAsync("Factorial", new Dictionary<string, object> { ["numero"] = numero });
    var text = result.Content.ElementAt(0).Text;
    return long.Parse(text);
}

// Ejecutar la herramienta 'Factorial' como ejemplo.
var result = await cliente.CallToolAsync( "Factorial", new Dictionary<string, object> { ["numero"] = 10 });
var text = result.Content.ElementAt(0).Text;
Console.WriteLine($"{result}");
// Imprimir el resultado.
Console.WriteLine($"\nResultado Factorial(10) = {text}");
Console.WriteLine($"\nR2 : {await Factorial(10)}");