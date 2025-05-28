# 🛠️ Plan de Trabajo: Aplicación de Contadores Múltiples en Blazor WebAssembly

## 🎯 Objetivo General

Crear una aplicación Blazor WebAssembly minimalista y responsiva para móviles, que permita:

- Agregar y eliminar contadores.
- Navegar entre páginas.
- Usar servicios para compartir estado.
- Vincular datos con `@bind`.
- Manejar eventos como `@onclick`.
- Medir clicks por tiempo transcurrido.

---

## ✅ Etapa 1: Estructura general del proyecto

- Crear proyecto Blazor WebAssembly vacío.
- Eliminar componentes innecesarios (`Counter.razor`, `FetchData.razor`, etc.).
- Dejar solo:
  - `App.razor`
  - `MainLayout.razor`
  - `Index.razor` (o reemplazar por `SetupPage.razor`)
- Definir 3 páginas:
  - `SetupPage.razor`: ingreso de contadores.
  - `ContadorPage.razor`: conteo con timer.
  - `ResumenPage.razor`: resultados y velocidad.

---

## ✅ Etapa 2: Modelo y Servicio de Estado

### Clase `Contador`:

```csharp
public class Contador
{
    public string Nombre { get; set; } = "";
    public int Cantidad { get; set; } = 0;
    public void Incrementar() => Cantidad++;
}
```

### Servicio `ContadorService`:

```csharp
public class ContadorService
{
    public List<Contador> Contadores { get; } = new();
    public DateTime? InicioConteo { get; private set; }

    public void IniciarConteo() => InicioConteo = DateTime.Now;
    public TimeSpan TiempoTranscurrido() => 
        InicioConteo is null ? TimeSpan.Zero : DateTime.Now - InicioConteo.Value;

    public void Reiniciar()
    {
        Contadores.Clear();
        InicioConteo = null;
    }
}
```

- Registrar como `Singleton` en `Program.cs`.

---

## ✅ Etapa 3: Primera página – Crear contadores

- Input con `@bind` para ingresar nombre del contador.
- Botón `Agregar` con `@onclick`.
- Lista con botón "❌" para eliminar cada contador.
- Botón "Iniciar" → navega a `/contador`.

---

## ✅ Etapa 4: Segunda página – Contar con tiempo

- Temporizador regresivo de 10 minutos.
- Mostrar cada contador con su botón `+1`.
- Deshabilitar botones al terminar el tiempo.
- Botón `Finalizar` o final automático → navega a `/resumen`.

---

## ✅ Etapa 5: Tercera página – Resumen

- Mostrar tabla con:
  - Nombre
  - Valor final
  - Velocidad (`(Valor / segundos) * 3600`)
- Botón `Reiniciar` que:
  - Llama a `ContadorService.Reiniciar()`
  - Redirige a `/`

---

## ✅ Etapa 6: Diseño Mínimo y Responsivo

- Eliminar Bootstrap.
- CSS limpio y adaptado a móviles.
- Tipografía clara.
- Botones grandes y espaciados.
- Nada innecesario visualmente.

---

## ✅ Etapa 7: Testeo y Mejora

- Verificar navegación, estado persistente, reinicio.
- Asegurar usabilidad en móviles.
- Ajustar estética y performance.
- Opcional: mejora progresiva con animaciones o persistencia local.

---

## 📦 Resultado Esperado

Una app Blazor WASM sin dependencias externas, con:
- Navegación fluida.
- UI optimizada para celulares.
- Estado compartido por servicio.
- Datos dinámicos y control total de eventos.

## IMPORTANTE
- Mantener el código limpio y modular.
- Comentar adecuadamente.
- Seguir buenas prácticas de desarrollo Blazor.
- Hacer este proyecto en forma totalmente autónoma sin revisar el código de otros.
- Crear el proyecto en la carpeta D5