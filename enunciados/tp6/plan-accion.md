# Plan de Acción — Tienda Online (Blazor WASM + Minimal API + EF Core/SQLite)

## 1. Configuración Inicial
- Crear solución con dos proyectos:
  - `TiendaOnline.Client` (Blazor WebAssembly)
  - `TiendaOnline.Server` (Minimal API)
- Configurar referencias y dependencias:
  - **Server:** EF Core, SQLite
  - **Client:** Bootstrap 5 o Tailwind, HttpClient

## 2. Diseño del Modelo de Datos
- Definir entidades:
  - `Product` (Id, Nombre, Descripción, Precio, Stock, Imagen)
  - `CartItem` (Id, ProductId, Cantidad, PrecioUnitario)
  - `Order` (Id, Nombre, Apellido, Email, Fecha, Total)
  - `OrderItem` (Id, OrderId, ProductId, Cantidad, PrecioUnitario)
- Configurar `DbContext` y relaciones
- Crear migraciones iniciales

## 3. Backend — Minimal API
- Endpoints CRUD:
  - `GET /api/products` (listado, búsqueda, stock)
  - `POST /api/cart/items` (agregar al carrito)
  - `PATCH /api/cart/items/{id}` (modificar cantidad)
  - `DELETE /api/cart/items/{id}` (eliminar del carrito)
  - `DELETE /api/cart` (vaciar carrito)
  - `POST /api/orders` (confirmar compra)
- Validaciones de stock y datos
- Manejo de errores y respuestas HTTP
- Seed inicial de 10 productos reales (con imágenes)
- Comando CLI para seed: `dotnet run --seed`

## 4. Frontend — Blazor WASM
- Servicios para consumir la API:
  - `ProductService`, `CartService`, `OrderService`
- Componentes principales:
  - Header (logo, buscador, carrito con contador)
  - ProductCard (ficha de producto)
  - CartItem (ítem del carrito)
- Páginas:
  1. Catálogo de productos (búsqueda, paginación, agregar al carrito)
  2. Carrito (modificar cantidades, vaciar, confirmar)
  3. Checkout (resumen, formulario cliente, confirmar)
- Validaciones y feedback visual
- UI responsive y accesible

## 5. Funcionalidades Avanzadas
- Persistencia local del carrito (localStorage)
- Sincronización del contador de carrito
- Validación de stock en tiempo real
- Mensajes de error y confirmación

## 6. Testing y Refinamiento
- Pruebas funcionales de flujo completo
- Optimización de consultas y performance
- Validación de accesibilidad

## 7. Documentación y Entrega
- README con instrucciones de instalación, migraciones y seed
- Documentación de la API
- Commit final etiquetado `v1.0`
- Verificación de funcionamiento desde cero
- Preparar defensa oral

---

**Cronograma sugerido:**
- Días 1-2: Setup y modelo
- Días 3-5: Backend
- Días 6-9: Frontend
- Día 10: Testing, documentación y entrega

**Tecnologías:** .NET 8+, Blazor WASM, Minimal API, EF Core, SQLite, Bootstrap 5
