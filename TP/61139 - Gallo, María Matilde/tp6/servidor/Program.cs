using Microsoft.EntityFrameworkCore;
using Servidor.Data;
using Servidor.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configurar DbContext con SQLite
builder.Services.AddDbContext<TiendaContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger para documentación de API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para permitir llamadas desde Blazor WebAssembly (localhost:5177)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
        policy.WithOrigins("http://localhost:5177")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Usar CORS
app.UseCors("AllowClientApp");

// Activar Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// Crear base de datos si no existe
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TiendaContext>();
    db.Database.EnsureCreated();
}

// Mapear endpoints personalizados
app.MapProductos();
app.MapCarrito();
app.MapCompras();

// Endpoint raíz para verificar que el servidor está corriendo
app.MapGet("/", () => "Servidor API está funcionando");

app.Run();
