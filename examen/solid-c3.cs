// SOLID
interface IProducto {
    int Id {get; set;}
    string Nombre {get; set;}
}

interface IProductoValorado {
    double Costo {get; set;}
    double Precio {get; set;}
}

class Producto : IProducto, IProductoValorado {
    public int Id { get; set; }
    public string Nombre { get; set; }

    public double Costo {get; set;}
    public double Precio {get; set;}
    public Producto(int id, string nombre, double costo, double precio) {
        Id = id;
        Nombre = nombre;
        Costo = costo;
        Precio = precio;
    }
}

class ProductoPerecedero : Producto {
    public DateTime FechaVencimiento { get; set; }

    public ProductoPerecede    public double Costo {get; set;}
ro(int id, string nombre, DateTime fechaVencimiento) : base(id, nombre) {
        FechaVencimiento = fechaVencimiento;
    }
}

class ProductoDigital : Producto {
    public string Version { get; set; }
    public ProductoDigital(int id, string nombre, string version) :
        base(id, nombre){
        Version = version;
    }
}

interface INotificar{
    void Info(string mensaje);
}

class Notificar : INotificar {
    public void Info(string mensaje) {
        Console.WriteLine(mensaje);
        File.AppendAllText("notificaciones.log", mensaje + Environment.NewLine);
    }
}

class NotificarEmail : INotificar {
    public void Info(string mensaje) {
        // Simulación de envío de correo electrónico
        Console.WriteLine($"Enviando email: {mensaje}");
    }
}

interface IRepositorioConsulta {
    int ConsultarStock(Producto producto);
}
interface IRepositorioActualizar {
    void ActualizarStock(Producto producto, int cantidad);
}

interface IRepositorio : IRepositorioActualizar, IRepositorioConsulta {
}

class Repositorio : IRepositorio {
    private Dictionary<int, int> stock = new();

    public int ConsultarStock(Producto producto) {
        return stock.TryGetValue(producto.Id, out int cantidad) ? cantidad : 0;
    }

    public void ActualizarStock(Producto producto, int cantidad){
        stock[producto.Id] = cantidad;
    }
}

class RepositorioFile : IRepositorio {
    private readonly string filePath = "stock.txt";
    private Dictionary<int, int> stock = new();

    public RepositorioFile() {
        if (File.Exists(filePath)) {
            foreach (var line in File.ReadAllLines(filePath)) {
                var parts = line.Split(',');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int id) &&
                    int.TryParse(parts[1], out int cantidad)) {
                    stock[id] = cantidad;
                }
            }
        }
    }

    public int ConsultarStock(Producto producto) {
        return stock.TryGetValue(producto.Id, out int cantidad) ? cantidad : 0;
    }

    public void ActualizarStock(Producto producto, int cantidad) {
        stock[producto.Id] = cantidad;
        File.WriteAllLines(filePath, stock.Select(kvp => $"{kvp.Key},{kvp.Value}"));
    }
}
interface IPolitica {
    bool DebeNotificar(Producto producto, int cantidad);
    string Mensaje(Producto producto, int cantidad);
}

class ReponerLimite : IPolitica {
    int Limite { get; set; }
    public ReponerLimite(int limite) {
        Limite = limite;
    }
    public override bool DebeNotificar(Producto producto, int cantidad) {
        return cantidad < Limite;
    }
    public override string Mensaje(Producto producto, int cantidad) {
        return $" [ATENCION] Stock bajo de {producto.Nombre}. Stock actual: {cantidad}";
    }
}

class ReponerNegativo : IPolitica {
    public override bool DebeNotificar(IProducto producto, int cantidad) {
        return cantidad < 0;
    }
    public override string Mensaje(IProducto producto, int cantidad) {
        return $" [ATENCION] Stock negativo de {producto.Nombre}. Stock actual: {cantidad}";
    }
}

class ReponerVencimiento : IPolitica {
    public int Plazo { get; set; }
    public ReponerVencimiento(int plazo) {
        Plazo = plazo;
    }
    public override bool DebeNotificar(Producto producto, int cantidad) {
        if (producto is ProductoPerecedero pp) {
            return pp.FechaVencimiento < DateTime.Now.AddDays(Plazo);
        }
        return false;
    }
    public override string Mensaje(Producto producto, int cantidad) {
        return $" [ATENCION] Producto perecedero {producto.Nombre} ha vencido. Fecha de vencimiento: {((ProductoPerecedero)producto).FechaVencimiento}";
    }
}

class ReponerOferta : IPolitica {
    public override bool DebeNotificar(Producto producto, int cantidad) {
        return cantidad < 5;
    }
    public override string Mensaje(Producto producto, int cantidad) {
        return $" [ATENCION] Stock bajo de {producto.Nombre}. Stock actual: {cantidad}";
    }
}


class Sistema {
    INotificar Notificar {get, private set;}
    IRepositorio Repositorio {get; private set;}
    IPolitica[] Politicas {get; private set}

    public Sistema(IRepositorio repositorio, IPolitica[] politicas, INotificar notificar){
        Repositorio = repositorio;
        Politicas = politicas;
        Notificar = notificar;
    }

    void AjustarStock(Producto producto, int cantidad){
        var actual = Repositorio.ConsultarStock(producto);
        var nuevo = actual + cantidad;
        Repositorio.ActualizarStock(producto, nuevo);

        foreach(var p in Politicas){
            if(p.DebeNotificar(producto, nuevo))
                Notificar.Info(p.Mensaje(producto, nuevo));
        }
  }

    public void Ejecutar(){
        var r = new Producto(1, "Reloj");
        var p = new ProductoPerecedero(2, "Pera", DateTime.Now.AddDays(5));
        var m = new ProductoDigital(3, "Microsoft Windows", "11");
        AjustarStock(r, 10);
        AjustarStock(p, 5);
        AjustarStock(m, 1000);


        WriteLine("=== Demostracion de Uso ===\n");
        WriteLine($"Stock {r.Nombre} antes: {repositorio.ConsultarStock(r)}");
        AjustarStock(r, -5);
        WriteLine($"Stock {r.Nombre} después: {repositorio.ConsultarStock(r)}\n");
   
        WriteLine($"Stock {p.Nombre} antes: {repositorio.ConsultarStock(p)}");
        AjustarStock(p, -8);
        WriteLine($"Stock {p.Nombre} después: {repositorio.ConsultarStock(p)}");

        WriteLine($"Stock {m.Nombre} antes: {repositorio.ConsultarStock(m)}");
        AjustarStock(m, -8);
        WriteLine($"Stock {m.Nombre} después: {repositorio.ConsultarStock(m)}");
    }
}

class NotificarMoke : INotificar {
    public bool Enviado = false;
    public void Info(string Mensaje){
        Enviado = true;
    }
}

var notificar = new NotificarFake(); 
var s = new Sistema(new Repositorio(),
        {
            new ReponerLimite(20),
            new ReponerNegativo(),
            new ReponerVencimiento(7)
        },
        notificar);

s.Ejecutar();
WriteLine(notificar.Enviado);