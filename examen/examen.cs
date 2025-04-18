using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using static System.Console;

using System.Security.Cryptography;
using System.Text;

// static int codificar(int pregunta, int respuesta) {
//     string input = $"{pregunta}-{respuesta}";
//     using var sha = SHA256.Create();
//     byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
//     int value = ((hash[0] << 8) | hash[1]) % 10000;
//     return value < 0 ? -value : value;
// }
static int codificar(int pregunta, int respuesta) {
    string input = $"{pregunta}-{respuesta}";
    return Math.Abs(input.GetHashCode() % 1000);
}

static int decodificar(int codificado,  int pregunta) {
    for(var r = 1; r <= 3; r++) {
        if (codificar(pregunta, r) == codificado) return r;
    }
    return 0;
}

static void EsperarTecla(){
        Write("Presione una tecla para continuar...");
        ReadKey(true);
        Write($"\r{' ',40}\r");
    }

static int LeerNumero(string prompt) {
    Write(prompt);
    string input = ReadLine()?.Trim() ?? "";
    
    if (int.TryParse(input, out int result))
        return result;
    return 0;
}

static int ObtenerParametro(int indice, int asumir = 0){
    string[] args = Environment.GetCommandLineArgs().Skip(2).ToArray();
    if (args == null || indice < 0 || indice >= args.Length)
        return asumir;
        
    if (!int.TryParse(args[indice], out int valor))
        return asumir;
        
    return valor;
}

// Método auxiliar para obtener el path del archivo fuente
string Ubicar(string nombre) {
    string GetSourceFilePath([System.Runtime.CompilerServices.CallerFilePath] string path = null) => path;
    string baseDir = Path.GetDirectoryName(GetSourceFilePath());
    string path = Path.Combine(baseDir, nombre);
    if (File.Exists(path)) return path;
    return Path.Combine(Environment.CurrentDirectory, nombre);
}

// Clase Pregunta
public class Pregunta {
    public int Numero { get; set; } = 0;
    public string Texto { get; set; } = "";
    public string[] Respuestas { get; set; } = new string[3];
    public int Correcta { get; set; } = 0; // Respuesta correcta (1, 2 o 3)
    public int Respuesta { get; set; } = 0;
    
    public bool EsRespondida => Respuesta != 0;
    public bool EsIncorrecta => Respuesta != Correcta;
    public bool EsCorrecta   => Respuesta == Correcta;
    
    public override string ToString() =>
        $"""
    
        ### {Numero:D3} {codificar(Numero, Correcta):D4}
        {Texto}
        
        a) {Respuestas[0]}
        b) {Respuestas[1]}
        c) {Respuestas[2]}
        
        """;
    
    // Reemplaza el Mostrar() anterior
    public void Mostrar(bool numerico = false, bool solucion = false){
        string f(string texto) => texto.Replace("```csharp", "").Replace("```", "").Replace("`", "");

        string opciones = numerico ?  "123" : "abc";
        int[] respuestas = solucion ? [Correcta - 1] : [0, 1, 2];
        WriteLine($"""
    
        ### {Numero:D3} 
        {f(Texto)}

        """);

        foreach(var r in respuestas)
            WriteLine($"{opciones[r]}) {f(Respuestas[r])}");
    }
}

// Clase contenedora de Preguntas
public class Preguntas : IEnumerable<Pregunta>  {
    List<Pregunta> Lista { get; set; } = new();

    public void CargarPreguntas(string Origen){
        var lineas = System.IO.File.ReadAllLines(Origen);
        var respuesta = -1;

        Pregunta pregunta = null;
        foreach (var linea in lineas) {

            if(linea.StartsWith("# ")) continue;
            if(linea.Trim() == "") continue;
            if(linea.Trim() == "---") continue;

            if(linea.StartsWith("###")) {
                if(pregunta != null) Lista.Add(pregunta);
                
                pregunta = new Pregunta();
                pregunta.Numero   = int.Parse(linea.Split(" ")[1]);
                pregunta.Correcta = decodificar(int.Parse(linea.Split(" ")[2]), pregunta.Numero);
                respuesta = -1;

                continue;
            };

            if (linea.StartsWith("a)") || linea.StartsWith("b)") || linea.StartsWith("c)")) {
                respuesta = linea[0] - 'a';
                pregunta.Respuestas[respuesta] = linea.Substring(2);
                continue;
            }

            if(respuesta < 0){
                pregunta.Texto += "\n" + linea;
            } else {
                pregunta.Respuestas[respuesta] += "\n" + linea;
            }
        }
        
        Lista.Add(pregunta);
    }   

    public void CargarRespuestas(string origen){
        var lineas = System.IO.File.ReadAllLines(origen);
        foreach (var linea in lineas) {
            if(!linea.Contains(". ")) continue;

            var partes = linea.Split(". ");
            var numero = int.Parse(partes[0]);
            var respuesta = partes[1][^1] - 'a' + 1;

            var pregunta = this.FirstOrDefault(p => p.Numero == numero);
            if(pregunta == null) continue;
            pregunta.Correcta = respuesta;
        }
    }

    public void CargarResultados(string origen) {
        if (!File.Exists(origen)) return;

        var lineas = File.ReadAllLines(origen);
        foreach (var linea in lineas)
        {
            if (!linea.Contains(". ")) continue;

            var partes = linea.Split(new[] {". "}, 2, StringSplitOptions.None);
            if (!int.TryParse(partes[0], out var numero)) continue;

            // Remover posible '*' al final y tomar la letra de respuesta
            var letra = partes[1].TrimEnd('*').Trim();
            if (string.IsNullOrEmpty(letra)) continue;

            var respuestaChar = letra[^1];
            var respuestaNum = respuestaChar - 'a' + 1;

            var pregunta = this.FirstOrDefault(p => p.Numero == numero);
            if (pregunta != null) {
                pregunta.Respuesta = respuestaNum;
            }
        }
    }

    public void GuardarPreguntas(string destino){
        using var sw = new StreamWriter(destino);
        
        sw.WriteLine("# Preguntas para el 1er Parcial");
        foreach (var pregunta in this) {
            sw.WriteLine(pregunta.ToString());
            sw.WriteLine("---");
        }
        sw.Close();
    }

    public void GuardarRespuestas(string destino){
        using var sw = new StreamWriter(destino);
        foreach (var pregunta in this.OrderBy(p => p.Numero)) {
            sw.WriteLine($"{pregunta.Numero:D3}. {(char)(pregunta.Correcta + 'a' - 1)}");
        }
        sw.Close();
    }

    public void ValidarNumeracion() {
        int n = 1;
        foreach (var p in Lista) {
            while (n < p.Numero) {
                WriteLine($"Falta {n}");
                n++;
            }
            n++;
        }
    }

    // Validar consistencia de preguntas
    public void Renumerar(){
        int n = 1;
        foreach (var p in Lista) {
            if (p.Numero != n) {
                WriteLine($"Renumerando {p.Numero} a {n}");
                p.Numero = n;
            }
            n++;
        }
    }

    public bool Validar() {
        bool valido = true;
        // Verificar numeración consecutiva
        int esperado = 1;
        foreach (var q in Lista.OrderBy(p => p.Numero)) {
            if (q.Numero != esperado) {
                WriteLine($"Error en {nameof(Validar)}: Falta la pregunta número {esperado}.");
                valido = false;
                esperado = q.Numero;
            }
            esperado++;
        }
        // Verificar que haya exactamente 3 respuestas y estén definidas
        foreach (var q in Lista) {
            if (q.Respuestas == null || q.Respuestas.Length != 3) {
                WriteLine($"Error en {nameof(Validar)}: La pregunta {q.Numero} no tiene exactamente 3 respuestas.");
                valido = false;
            } else {
                for (int i = 0; i < 3; i++) {
                    if (string.IsNullOrWhiteSpace(q.Respuestas[i])) {
                        WriteLine($"Error en {nameof(Validar)}: La pregunta {q.Numero} tiene la respuesta {i+1} vacía.");
                        valido = false;
                    }
                }
            }
            // Verificar que la respuesta correcta esté en 1..3
            if (q.Correcta < 1 || q.Correcta > 3) {
                WriteLine($"Error en {nameof(Validar)}: La pregunta {q.Numero} tiene un índice de respuesta correcta inválido ({q.Correcta}).");
                valido = false;
            }
        }
        return valido;
    }

    // Implementación de IEnumerable<Pregunta>
    public IEnumerator<Pregunta> GetEnumerator() => Lista.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Pregunta> Pendientes() => this.Where(p => !p.EsRespondida);
    public IEnumerable<Pregunta> Respondidas() => this.Where(p => p.EsRespondida);
    public IEnumerable<Pregunta> Correctas() => this.Where(p => p.EsRespondida && p.EsCorrecta);
    public IEnumerable<Pregunta> Incorrectas() => this.Where(p => p.EsRespondida && p.EsIncorrecta);
    public int Count => Lista.Count;

}


// Clase Examen
class Examen {
    public List<Pregunta> Preguntas { get; set; } = new();
    public bool MostrarNumeros { get; set; } = false;
    private Preguntas Base { get; set; }

    private DateTime HoraInicio; // Registrar hora de inicio

    public Examen(Preguntas preguntas, int cantidad){
        Base = preguntas;
        HoraInicio = DateTime.Now; // Guardar hora de inicio

        var origen = cantidad < 0 ? preguntas.Incorrectas() : preguntas.Pendientes();
        cantidad = Math.Abs(cantidad);
        cantidad = origen.Count() < cantidad ? origen.Count() : cantidad;
        if (cantidad == 0) {
            WriteLine("\n🎉 Felicitaciones. Respondiste todas las preguntas. 🎉\n\n");
            Environment.Exit(0); // Terminar el programa
            return;
        }
        
        Random random = new Random();
        Preguntas = origen
            .OrderBy(x => random.Next())
            .Take(cantidad)
            .OrderBy(p => p.Numero)
            .ToList();
        Preguntas.ForEach( p => p.Respuesta = 0);
    }

    static bool IsNumericKey(ConsoleKey key) {
        return key == ConsoleKey.D1 || key == ConsoleKey.NumPad1 ||
               key == ConsoleKey.D2 || key == ConsoleKey.NumPad2 ||
               key == ConsoleKey.D3 || key == ConsoleKey.NumPad3;
    }
    
    static bool IsLetterKey(ConsoleKey key) {
        return key == ConsoleKey.A || key == ConsoleKey.B || key == ConsoleKey.C;
    }
    
    public int Nota() =>
        Preguntas.Count(p => p.Respuesta == p.Correcta);

    public void GuardarRespuestas(string destino) {
        using var sw = new StreamWriter(destino);
        foreach (var p in Base.Respondidas().OrderBy(p => p.Numero)) {
            var mark = p.EsCorrecta ? "" : "*";
            sw.WriteLine($"{p.Numero:D3}. {(char)(p.Respuesta + 'a' - 1)}{mark}");
        }
    }   

    // Cambiar firma para recibir legajo
    public void Evaluar(int legajo){
        string FormatResponse(int responseNum) => 
                    MostrarNumeros 
                        ? responseNum.ToString() 
                        : ((char)('a' + responseNum - 1)).ToString();
        
        var todas = Preguntas.ToList();
        int cantidad = Preguntas.Count();
        int actual = 0;
        bool abortar = false;
        TimeSpan limite = TimeSpan.FromMinutes(20);

        while (actual < Preguntas.Count()) {
            DateTime tickInicio = DateTime.Now;
            ConsoleKeyInfo? key = null;
            int lastSecond = -1;
            Pregunta preguntaActual = Preguntas[actual];

            while (key == null) {
                TimeSpan transcurrido = DateTime.Now - HoraInicio;
                TimeSpan restante = limite - transcurrido;
                if (restante < TimeSpan.Zero) restante = TimeSpan.Zero;

                int segRest = (int)restante.TotalSeconds;
                if (lastSecond != segRest) {
                    Clear();
                    WriteLine($"- Pregunta {actual + 1,2} de {Preguntas.Count} --- Legajo: {legajo} - Examen 1er Parcial - Tiempo restante: {restante.Minutes:D2}:{restante.Seconds:D2} -\n");
                    preguntaActual.Mostrar(numerico: MostrarNumeros);
                    if (preguntaActual.Respuesta != 0) {
                        WriteLine($"\nRespuesta: {FormatResponse(preguntaActual.Respuesta)}\n");
                    } else {
                        WriteLine("\n\n\n");
                    }
                    Write("Respuesta (←/→ navegar, a/b/c o 1/2/3 para responder): ");
                    lastSecond = segRest;
                }

                if ((DateTime.Now - HoraInicio) >= limite) {
                    WriteLine("\n\nSe acabó el tiempo del examen.");
                    EsperarTecla();
                    actual = Preguntas.Count; // salir del bucle principal
                    break;
                }

                if (KeyAvailable) {
                    key = ReadKey(true);
                } else {
                    System.Threading.Thread.Sleep(100);
                }
            }
            if (key == null) break; // por si se acabó el tiempo

            var k = key.Value.Key;
            if (IsNumericKey(k) || IsLetterKey(k)) MostrarNumeros = IsNumericKey(k);

            int resp = k switch {
                ConsoleKey.A or ConsoleKey.D1 or ConsoleKey.NumPad1 => 1,
                ConsoleKey.B or ConsoleKey.D2 or ConsoleKey.NumPad2 => 2,
                ConsoleKey.C or ConsoleKey.D3 or ConsoleKey.NumPad3 => 3,
                _ => 0
            };
            
            if (resp > 0) {
                Preguntas[actual].Respuesta = resp;
                actual++;
                continue;
            }
            
            switch (k) {
                case ConsoleKey.Escape:
                case ConsoleKey.X:
                    abortar = true;
                    actual = Preguntas.Count;
                    break;
                case ConsoleKey.LeftArrow:
                    if (actual > 0) actual--;
                    break;
                case ConsoleKey.RightArrow:
                    if (actual < Preguntas.Count - 1) actual++;
                    break;
                default:
                    break;
            }
        }

        if (abortar) return;

        TimeSpan duracion = DateTime.Now - HoraInicio;

        Clear();
        WriteLine();
        var incorrectas = Preguntas.Where(p => p.EsIncorrecta);
        if (incorrectas.Any()) {
            WriteLine($"Hay {incorrectas.Count()} {(incorrectas.Count() == 1 ? "respuesta incorrecta" : "respuestas incorrectas")}\n");
            int i = 1;
            foreach(var p in incorrectas) {
                WriteLine($"{i++}) \n");
                WriteLine($"Tu respuesta fue {FormatResponse(p.Respuesta)} la correcta era {FormatResponse(p.Correcta)}\n");
                WriteLine("------------");
                p.Mostrar(solucion: true, numerico: MostrarNumeros);
                WriteLine("------------");
                EsperarTecla();
                Clear();
            }
        } else {
            WriteLine("¡Todas las respuestas fueron correctas! 🎉 Felicitaciones.");
        }
        Informar(duracion);
    }

    // Modificar para recibir duración
    private void Informar(TimeSpan? duracion = null){
        // Nota y porcentaje de la sesión
        Clear();
        var cantidad = Preguntas.Count;
        var total = Base.Count();
        var respondidas = Base.Respondidas().Count();
        var correctas   = Base.Correctas().Count();
        var incorrectas = Base.Incorrectas().Count();

        WriteLine($"""

        ### Examen 1er Parcial ###

            --- Último examen ---
                     Nota: {Nota()} de {cantidad};
               Porcentaje: {(Nota() * 100) / cantidad}%
            
            --- Evaluación Total ---
                Preguntas: {total,3}
              Respondidas: {respondidas,3}
                Correctas: {correctas,3}
              Incorrectas: {incorrectas,3} 
               Porcentaje: {(correctas * 100) / respondidas}%
             
        """);

        // Mostrar duración y segundos por pregunta si corresponde
        if (duracion != null) {
            double segundos = duracion.Value.TotalSeconds;
            double segPorPregunta = cantidad > 0 ? segundos / cantidad : 0;
            WriteLine($"    Tiempo total: {duracion.Value.Minutes:D2}:{duracion.Value.Seconds:D2} ({segundos:F1} segundos)");
            WriteLine($"    Segundos por pregunta: {segPorPregunta:F2}");
        }
    }
}


// --- EJECUCIÓN DEL EXAMEN ---

Clear();
WriteLine("\n\n### Examen 1er Parcial ###\n\n");

int legajo   = ObtenerParametro(0);
int cantidad = ObtenerParametro(1, 10);

while(legajo < 55000 || legajo > 65000) {
    legajo = LeerNumero("Ingrese número de legajo: ");
} 


var preguntas = new Preguntas();
preguntas.CargarPreguntas(Ubicar("preguntas-examen.md"));
preguntas.CargarRespuestas(Ubicar("respuestas-examen.md"));
preguntas.CargarResultados(Ubicar($"{legajo}.txt"));

preguntas.GuardarPreguntas(Ubicar("preguntas-examen.md"));

if(!preguntas.Validar()) {
    WriteLine("Error en la numeración de preguntas o respuestas.");
    preguntas.Renumerar();
    preguntas.GuardarPreguntas(Ubicar("preguntas-examen.md"));
    preguntas.GuardarRespuestas(Ubicar("respuestas-examen.md"));
    return;
}

var examen = new Examen(preguntas, cantidad);
examen.Evaluar(legajo);
examen.GuardarRespuestas(Ubicar($"{legajo}.txt"));

// Fin del programa