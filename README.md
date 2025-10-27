# CinePark

CinePark es un proyecto universitario que simula la gestión de un cine, incluyendo funciones, compras de entradas y control de salas. Está desarrollado usando **ASP.NET Core** para el backend y **HTML, CSS y JavaScript** para el frontend.

---

## Estructura del proyecto

```plaintext
.
├── frontend
│   ├── assets
│   │   ├── css
│   │   └── img
│   ├── index.html
│   ├── js
│   └── pages
├── Proyecto
│   ├── Controllers
│   ├── Data
│   ├── Migrations
│   ├── Models
│   ├── Services
│   ├── Program.cs
│   ├── Properties
│   ├── appsettings.json
│   └── Proyecto.sln



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
- Reproduccion de Trailers
- Roles por cuenta administratidor y cliente
- Integración con Cloudinary para manejo de imágenes
