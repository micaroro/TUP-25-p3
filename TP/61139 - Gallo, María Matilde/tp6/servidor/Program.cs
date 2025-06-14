using Microsoft.EntityFrameworkCore;
using Servidor.Data;
using Servidor.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<TiendaContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para Blazor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
    {
        policy.WithOrigins("http://localhost:5177") // Blazor WebAssembly
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowClientApp");
app.UseSwagger();
app.UseSwaggerUI();

// Crear BD 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TiendaContext>();
    db.Database.EnsureCreated();
}

// Endpoints
app.MapGet("/", () => "Servidor API está funcionando");
app.MapProductos();
app.MapCarrito();
 app.MapCompras(); 



app.Run();
