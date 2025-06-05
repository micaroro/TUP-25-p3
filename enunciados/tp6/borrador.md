# Segundo Parcial.

## Objetivo
Desarrollar una app web que integre frontend y backend usando c# en todo el stack.

Se debe presentar dos proyectos
1. Aplicacion de usuario.
Realizada con Blazor para WAsm que interaccion con el usuario y ejecuta la funcionalidad via llamadas al servidor.
2. Servidor de aplicacion.
Realizada con Minimal Api, debe implementar una API Rest que implemente la funcionalidad que demanda el front end y persiste los datos en la base de datos.
Se debe usar Entity Framework Core como ORM y SQLite como base de datos.

## Funcionalidad.
Debe realizar una aplicacion para gestiona una tienda online.

### Sistema en el cliente.

1. Seleccion de producto.
- Esa pantalla debe mostrar una lista de productos y un boton para agregar el producto al carrito.
- En la cabecera de la pagina debe tener:
    - El logo del sitio que al pulsarse vuelve siempre a esta pantalla.
    - Una entrada de datos que permita la busqueda.
    - Un icono de carrito que al pulsarlo me lleve al mismo. Este icono debe estar acompañado de las cantidad de unidades que hay en el mismo.
- En el cuerpo debe mostrar un listado de los producto en fichas.
    - Debe mostrar la imagen del producto.
    - Debe mostrar el nombre y descripcion del mismo
    - Debe tener el precio unitario y las unidades disponibles.
    - Debe tener un boton 'Agregar al carrito' que agrege al producto al carrito y me muestre el carrito.
    - Debe verificar haya existencia suficiente para realizar la compra.

2. Carrito de compra
- Esta pantalla debe mostrar todos los productos en el carrito, debe tener el total y un mecanismo para aumentar o disminuir la cantidad de items de un producto dato.
- Deme mostrar el listado de todos los productos en el carrito, la posibilidad de cambiar la contidad y un boton que me permita confirmar la compra, en cuyo caso debe llevarme a la pantalla 3.
- Para cada producto se debe mostrar el nombre, las unidades, el precio unitario y el importe total para cada producto.
- Tambien debe tener un boton que me permita aumentar o disminuir la cantidad (revisando y actualizando la existencia)
- Debe tener un boton 'Vaciar carrito' que al pulsarlo devuelve la unidades a catalogo generar (actualiza la existencia) y llevame a la pantalla.
- Debajo de la lista de item debe mostrar el total de la compra 
- Tambien debe tener un 'Confirmar compra' que lleve a la tercera pantalla.

3. Confirmar compra
- En esta pantalla se muestra en la parte superio un resumen de la informacion del carrito (unidades totales compradas e importe total)
- Se debe ingresar el nombre, apellido e email del cliente (todos campos obligatorios)
- Debe tener un boton 'Confirmar' que le lleva nuevamente a la pantalla principal.
- En la cabecear debe estar el logo y el carrito de manera que al pulsarno navegue a la pagina 1 o a la 2 segun corresponda.

### Sistema en el servidor

Debe implementar las funciones necesarias por el cliente.
- Listar productos (con la posibilida de buscar y controlando que tenga existencia)
- Agregar producto al carrito
- Eliminar producto al carrito.
- Limpiar carrito.
- Confirmar carrito (con en nombre, apellido y email)

Los datos de los productos, y de las compras debe ser persistidos usando Entity Framework usando SQLite.

El sistema inicialmente debe estar cargado con al menos 10 productos (con imagenes y valores realista) de la categoria que quiera. 





