namespace TUP;

public static class Consola {
    public static void Limpiar() {
        Console.Clear();
    }

    // Helper method to print colored text and reset color afterwards
    public static void Escribir(string text, ConsoleColor color = ConsoleColor.White) {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    
    // Helper method to wait for a key press
    public static void EsperarTecla(string mensaje = "\nPresione cualquier tecla para continuar...") {
        Console.Write(mensaje);
        Console.ReadKey(true);
        Console.WriteLine();
    }
    
    // Helper method to read a string input with optional validation
    public static string LeerCadena(string mensaje = "", string[]? valoresPosibles = null) {
        string entrada;
        bool entradaValida;
        
        do {
            Console.Write(mensaje ?? "");
            entrada = Console.ReadLine() ?? "";
            
            // Si no hay valores posibles específicos, cualquier entrada es válida
            if (valoresPosibles == null || valoresPosibles.Length == 0) {
                entradaValida = true;
            } else {
                entradaValida = valoresPosibles.Contains(entrada);
                if (!entradaValida) {
                    Console.WriteLine($"Entrada no válida. \nOpciones permitidas: {string.Join(", ", valoresPosibles)}");
                }
            }
        } while (!entradaValida);
        
        return entrada;
    }
    
    // Helper method to get a valid option from a range
    // Método para elegir una opción de un conjunto de valores posibles
    public static string ElegirOpcion(string mensaje, string valoresPosibles) {
        string[] opciones = new string[valoresPosibles.Length];
        for (int i = 0; i < valoresPosibles.Length; i++) {
            opciones[i] = valoresPosibles[i].ToString();
        }
        
        return LeerCadena(mensaje, opciones);
    }
    
    public static bool Confirmar(string mensaje) {
        ConsoleKeyInfo key;
        while(true) {
            Console.Write($"{mensaje} (S/N): ");
            key = Console.ReadKey(true); // true hides the pressed key
            char keyChar = char.ToUpper(key.KeyChar);
            Console.WriteLine(keyChar); // Show which key was pressed
            
            if (keyChar != 'S' && keyChar != 'N') {
                Escribir("Por favor, presione S o N.", ConsoleColor.Red);
                continue;
            }
            return keyChar == 'S';
        }
    }
}
