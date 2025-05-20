#!/usr/bin/env dotnet script
#r "sdk:Microsoft.NET.Sdk.Web"                    // top-level await, etc.
#r "nuget: ModelContextProtocol, 0.2.0-preview.1"

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;   // TransportTypes

// ─────────────────────────────────────────────
// Conecta al servidor MCP que arrancas como sub-proceso
// ─────────────────────────────────────────────
var client = await McpClientFactory.CreateAsync(new()
{
    Id            = "agenda-client",
    Name          = "Agenda Client",
    TransportType = TransportTypes.StdIo,
    TransportOptions = new()
    {
        ["command"]   = "dotnet",
        ["arguments"] = $"script /Users/adibattista/Documents/GitHub/tup-25-p3/clases/21.mcp.agenda.csx"
    }
});

// ─────────────────────────────────────────────
// Ejemplo de uso
// ─────────────────────────────────────────────
Console.WriteLine("— Herramientas disponibles —");
foreach (var tool in await client.ListToolsAsync())
    Console.WriteLine($"• {tool.Name} — {tool.Description}");

Console.WriteLine("\nInvocando obtenerContactos …");
var respuesta = await client.CallToolAsync("obtenerContactos", CancellationToken.None);

string texto = respuesta.Content.First(c => c.Type == "text").Text;
Console.WriteLine($"\nResultado:\n{texto}");