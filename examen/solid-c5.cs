// SOLID
// S: Single Responsibility Principle
// O: Open/Closed Principle
// L: Liskov Substitution Principle
// I: Interface Segregation Principle
// D: Dependency Inversion Principle

class IProducto {
    int Id { get; set; }
    string Nombre { get; set; }
}
class Producto : IProducto {
    public int Id { get; set; }
    public string Nombre { get; set; }

    public Producto(int id, string nombre) {
        Id = id;
        Nombre = nombre;
    }
}

class ProductoPerecedero : Producto {
    public DateTime FechaVencimiento { get; set; }

    public ProductoPerecedero(int id, string nombre, DateTime fechaVencimiento) : base(id, nombre) {
        FechaVencimiento = fechaVencimiento;
    }
}

interface INotificar {
    void Info(string mensaje);
}

class Notificar : INotificar {
    public virtual void Info(string mensaje) {
        Console.WriteLine(mensaje);
        File.AppendAllText("notificaciones.log", mensaje + Environment.NewLine);
    }
}

class NotificarEmail : Notificar {
    public string Email {get; set; }

    public NotificarEmail(string email){
        Email = email;
    }
    public override void Info(string Mensaje){
        WriteLine($"Simula enviar email :{mensaje} a {Email}")
    }
}
interface IRepositorioConsultar {
    int ConsultarStock(IProducto producto);
}
interface IRepositorioActualizar {
    void ActualizarStock(IProducto producto, int cantidad);
}
interface IRepositorio : IRepositorioConsultar, IRepositorioActualizar {
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

interface IPolitica {
    bool DebeNotificar(IProducto producto, int cantidad);
    string Mensaje(IProducto producto, int cantidad);
}

class ReponerUmbral : IPolitica {
    public int Umbral { get; set;}

    public ReponerUmbral(int umbral) : base(){
        Umbral = umbral;
    }

    public override bool DebeNotificar(IProducto producto, int cantidad) {
        return cantidad < Umbral;
    }
    public override string Mensaje(IProducto producto, int cantidad) {
        return $" [ATENCION] Stock bajo de {producto.Nombre}. Stock actual: {cantidad} (El minimo es {Umbral})";
    }
}

class ReponerNegativo : IPolitica {
    public override bool DebeNotificar(IProducto producto, int cantidad){
        return cantidad < 0 ;
    }
    public override string Mensaje(IProducto producto, int cantidad){
        return $" [ATENCION] Stock negativo de {producto.Nombre}. Stock actual: {nuevo}"
    }
}

class ReponerVencimiento : IPolitica {
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

class Sistema {
    IRepositorio Repositorio;
    IPoliticas[] Politicas;
    INotificar Notiticar;
    public Sistema(IRepositorio repositorio, IPolitica[] politicas, INotificar notificar){
        Repositorio = repositorio;
        Politicas = politica;
        Notificar = notificar;

    }
    void AjustarStock(Producto producto, int cantidad){
        var actual = Repositorio.ConsultarStock(producto);
        var nuevo = actual + cantidad;
        Repositorio.ActualizarStock(producto, nuevo);

        foreach(var p in Politicas){
            if(p.DebeNotificar(producto, nuevo)){
                Notificar.Info(p.Mensaje(producto, nuevo))
            }
        }
    }

    public void Ejecutar(){
        var r = new Producto(1, "Reloj");
        var p = new ProductoPerecedero(2, "Pera", DateTime.Now.AddDays(5));

        AjustarStock(r, 10);
        AjustarStock(p, 5);


        WriteLine("=== Demostracion de Uso ===\n");
        WriteLine($"Stock {r.Nombre} antes: {repositorio.ConsultarStock(r)}");
        AjustarStock(r, -5);
        WriteLine($"Stock {r.Nombre} después: {repositorio.ConsultarStock(r)}\n");
   
        WriteLine($"Stock {p.Nombre} antes: {repositorio.ConsultarStock(p)}");
        AjustarStock(p, -8);
        WriteLine($"Stock {p.Nombre} después: {repositorio.ConsultarStock(p)}");
    }
}

class RepositorioDB : IRepositorio {
    //
}

class NotificarTest : INotificar {
    public int Contador = 0;
    public void Info(string mensaje) {
        Contador++;
    }
}

var noti = new NotificarTest();
var s = new Sistema(
    new RepositorioMemoria(),
    {
        new ReponerUmbral(5),
        new ReponerNegativo(),
    },
    noti
);

s.Ejecutar();
if(noti.Contador == 3);