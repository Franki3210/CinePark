# CinePark

CinePark es un proyecto universitario que simula la gestión de un cine, incluyendo funciones, compras de entradas y control de salas. Está desarrollado usando **ASP.NET Core** para el backend y **HTML, CSS y JavaScript** para el frontend.

---

## Estructura del proyecto

```plaintext
.
├── frontend
│   ├── assets
│   │   ├── css
│   │   │   ├── carrusel.css
│   │   │   ├── funcion.css
│   │   │   ├── historial.css
│   │   │   ├── main.css
│   │   │   └── pelicula.css
│   │   └── img
│   │       ├── logo.jpeg
│   │       └── QR.png
│   ├── index.html
│   ├── js
│   │   ├── api.js
│   │   └── main.js
│   └── pages
│       ├── function.html
│       ├── movie.html
│       └── purchase_history.html
├── Proyecto
│   ├── Controllers
│   │   ├── administradorsController.cs
│   │   ├── AsientoesController.cs
│   │   ├── ComprasController.cs
│   │   ├── CuentasController.cs
│   │   ├── FuncionsController.cs
│   │   ├── PeliculasController.cs
│   │   ├── PersonasController.cs
│   │   └── SalasController.cs
│   ├── Data
│   │   └── Database.cs
│   ├── Migrations
│   │   ├── 20251026221413_m1.cs
│   │   ├── 20251026221413_m1.Designer.cs
│   │   └── DatabaseModelSnapshot.cs
│   ├── Models
│   │   ├── administrador.cs
│   │   ├── Asiento.cs
│   │   ├── Compra.cs
│   │   ├── Cuenta.cs
│   │   ├── Funcion.cs
│   │   ├── Pelicula.cs
│   │   ├── Persona.cs
│   │   ├── Sala.cs
│   │   └── SalaDetalleDTO.cs
│   ├── Services
│   │   └── CloudinaryService.cs
│   ├── Program.cs
│   ├── Properties
│   │   └── launchSettings.json
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Proyecto.csproj


```
## Tecnologías usadas

Backend: ASP.NET Core

Frontend: HTML, CSS, JavaScript

Base de datos: PostgreSQL (según la carpeta Migrations y Data)
```plaintext
Servicios: Cloudinary (para gestión de imágenes)
│   ├── Proyecto.csproj.user
│   └── Proyecto.sln
```
## Funcionalidades principales

- Gestión de películas y funciones
- Registro y manejo de cuentas de usuario
- Historial de compras
- Control de salas y asientos
- Integración con Cloudinary para manejo de imágenes
