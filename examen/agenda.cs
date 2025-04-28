// Archivo: agenda_turso.csx

#r "nuget: Microsoft.EntityFrameworkCore.Sqlite.Core,7.0.0"
#r "nuget: SQLitePCLRaw.bundle_e_sqlite3,2.1.4"
#r "nuget: Microsoft.EntityFrameworkCore,7.0.0"

using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

// Inicializar SQLite explícitamente
static void InitializeSqlite() {
    Batteries_V2.Init();
}

// Entidad Contact
public class Contact {
    public int    Id        { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName  { get; set; } = "";
    public string Phone     { get; set; } = "";
    public string Email     { get; set; } = "";
}

// DbContext apuntando a SQLite local
public class AppDbContext : DbContext {
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        var connectionString = new SqliteConnectionStringBuilder { DataSource = "agenda.db" }.ToString();
        options.UseSqlite(connectionString);
    }
}

void Main() {
    InitializeSqlite();
    using var db = new AppDbContext();
    db.Database.EnsureCreated();

    bool exit = false;
    while (!exit) {
        Console.WriteLine("\nAgenda - Turso");
        Console.WriteLine("1. Agregar contacto");
        Console.WriteLine("2. Listar contactos");
        Console.WriteLine("3. Editar contacto");
        Console.WriteLine("4. Borrar contacto");
        Console.WriteLine("5. Salir");
        Console.Write("Seleccione una opción: ");
        var opcion = Console.ReadLine();

        switch (opcion) {
            case "1": AddContact(db); break;
            case "2": ListContacts(db); break;
            case "3": EditContact(db); break;
            case "4": DeleteContact(db); break;
            case "5": exit = true; break;
            default: Console.WriteLine("Opción inválida."); break;
        }
    }
}

void AddContact(AppDbContext db) {
    Console.Write("Nombre: ");
    var fn = Console.ReadLine() ?? "";
    Console.Write("Apellido: ");
    var ln = Console.ReadLine() ?? "";
    Console.Write("Teléfono: ");
    var ph = Console.ReadLine() ?? "";
    Console.Write("Email: ");
    var em = Console.ReadLine() ?? "";

    var c = new Contact { FirstName = fn, LastName = ln, Phone = ph, Email = em };
    db.Contacts.Add(c);
    db.SaveChanges();
    Console.WriteLine($"Contacto agregado con Id: {c.Id}");
}

void ListContacts(AppDbContext db) {
    var list = db.Contacts.ToList();
    if (list.Any()) {
        foreach (var c in list)
            Console.WriteLine($"{c.Id}: {c.FirstName} {c.LastName} - {c.Phone} - {c.Email}");
    } else {
        Console.WriteLine("No hay contactos.");
    }
}

void EditContact(AppDbContext db) {
    Console.Write("Id del contacto a editar: ");
    if (!int.TryParse(Console.ReadLine(), out var id)) {
        Console.WriteLine("Id inválido.");
        return;
    }

    var c = db.Contacts.Find(id);
    if (c == null) {
        Console.WriteLine("Contacto no encontrado.");
        return;
    }

    Console.Write($"Nombre ({c.FirstName}): "); var fn = Console.ReadLine();
    Console.Write($"Apellido ({c.LastName}): "); var ln = Console.ReadLine();
    Console.Write($"Teléfono ({c.Phone}): "); var ph = Console.ReadLine();
    Console.Write($"Email ({c.Email}): "); var em = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(fn)) c.FirstName = fn;
    if (!string.IsNullOrWhiteSpace(ln)) c.LastName  = ln;
    if (!string.IsNullOrWhiteSpace(ph)) c.Phone     = ph;
    if (!string.IsNullOrWhiteSpace(em)) c.Email     = em;

    db.SaveChanges();
    Console.WriteLine("Contacto actualizado.");
}

void DeleteContact(AppDbContext db) {
    Console.Write("Id del contacto a borrar: ");
    if (!int.TryParse(Console.ReadLine(), out var id)) {
        Console.WriteLine("Id inválido.");
        return;
    }

    var c = db.Contacts.Find(id);
    if (c == null) {
        Console.WriteLine("Contacto no encontrado.");
        return;
    }

    db.Contacts.Remove(c);
    db.SaveChanges();
    Console.WriteLine("Contacto eliminado.");
}

// Inicia la aplicación
Main();
