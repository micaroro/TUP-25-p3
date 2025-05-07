
WriteLine("Ejemplo de excepciones en C#");

void PruebaExcepcion() {
    try {
        int divisor = 0;
        int resultado = 10 / divisor; // Esto lanzará una excepción de división por cero
    } catch (DivideByZeroException ex) {
        Console.WriteLine("Se produjo una excepción: " + ex.Message);
    } finally {
        Console.WriteLine("Bloque finally ejecutado.");
    }
}

// PruebaExcepcion();

void FuncionQueLanzaExcepcion() {
    WriteLine("2. Antes de la excepcion...");
    throw new Exception("Esta es una excepción lanzada intencionadamente.");
    WriteLine("3. Después de la excepcion...");
}

void FuncionPrincipal(){
    WriteLine("1. Antes de la excepcion...");
    FuncionQueLanzaExcepcion();
    WriteLine("4. Después de la excepcion...");
}

void PruebaExcepcionAnidada() {
    try {
        FuncionPrincipal();
    } catch (Exception ex) {
        Console.WriteLine("Se produjo una excepción: " + ex.Message);
    } finally {
        Console.WriteLine("Bloque finally ejecutado.");
    }
}
// PruebaExcepcionAnidada();

// Ejemplo usando throw como expresión condicional
string ObtenerNombre(string nombre) {
    // Si nombre es null, lanza una excepción usando throw como expresión
    return nombre ?? throw new ArgumentNullException(nameof(nombre), "El nombre no puede ser null.");
}

// Prueba del método
try {
    string nombre = ObtenerNombre(null);
    Console.WriteLine($"Nombre: {nombre}");
} catch (ArgumentNullException ex) {
    Console.WriteLine($"Excepción capturada: {ex.Message}");
}