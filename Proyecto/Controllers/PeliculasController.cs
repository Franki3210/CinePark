using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;
using Proyecto.Services;
namespace Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly Database _context;
        private readonly CloudinaryService _cloudinaryService;

        public PeliculasController(Database context, CloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }


        // Obtener todas las películas activas
        [HttpGet("obtener")]
        public async Task<ActionResult<IEnumerable<Pelicula>>> GetPeliculas()
        {
            try
            {
                return await _context.Peliculas
                    .Where(p => p.Estado == "activo")
                    .ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        // Obtener película por ID
        [HttpGet("obtenerporId")]
        public async Task<ActionResult<Pelicula>> GetPelicula(int id)
        {
            try
            {
                var pelicula = await _context.Peliculas.FindAsync(id);

                if (pelicula == null || pelicula.Estado == "borrado")
                    return NotFound(new { mensaje = "No se encontró la película" });

                return pelicula;
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }
        // EN: PeliculasController.cs

        [HttpGet("obtenerParaAdmin")]
        public async Task<ActionResult<Pelicula>> ObtenerPeliculaParaAdmin(int id)
        {
            try
            {
                // ✅ ¡ESTE ES EL CAMBIO CLAVE!
                // 1. Usamos .IgnoreQueryFilters() para decirle a EF que ignore el filtro global de "borrado".
                // 2. Usamos .FirstOrDefaultAsync() para buscar la película por su ID.
                var pelicula = await _context.Peliculas
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.IdPelicula == id);

                // La validación simple se mantiene igual.
                if (pelicula == null)
                {
                    return NotFound(new { mensaje = $"No se encontró ninguna película con el ID {id} (incluso buscando entre las borradas)." });
                }

                // Se devuelve sin importar el estado.
                return Ok(pelicula);
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor al buscar la película." });
            }
        }

        // Actualizar película
        [HttpPut("actualizar")]
        public async Task<IActionResult> ActualizarPelicula(string tituloActual, string nuevoTitulo)
        {
            try
            {
                // Buscar la película por su título actual
                var peliculaActual = await _context.Peliculas
                    .FirstOrDefaultAsync(p => p.Titulo.ToLower() == tituloActual.ToLower() && p.Estado.ToLower() != "borrado");

                if (peliculaActual == null)
                    return NotFound(new { mensaje = $"No se encontró la película con el título '{tituloActual}'" });

                // Actualizar el título si se envió un nuevo valor
                if (!string.IsNullOrEmpty(nuevoTitulo))
                {
                    peliculaActual.Titulo = nuevoTitulo;
                    _context.Peliculas.Update(peliculaActual);
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        mensaje = $"Película actualizada correctamente.",
                        id = peliculaActual.IdPelicula,
                        tituloAnterior = tituloActual,
                        tituloNuevo = nuevoTitulo
                    });
                }

                return BadRequest(new { mensaje = "Debe proporcionar un nuevo título para actualizar." });
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }


        // Registrar nueva película
        [HttpPost("registrar")]
        public async Task<ActionResult<Pelicula>> PostPelicula(string titulo, int duracion, string genero, string estado, string restriccionEdad, string sipnosis, DateOnly fechaEstreno)
        {
            try
            {
                if (await _context.Peliculas.AnyAsync(p => p.Titulo.ToLower() == titulo.ToLower() && p.Estado == "activo"))
                    return BadRequest(new { mensaje = "Esta película ya fue creada" });

                var nuevaPelicula = new Pelicula
                {
                    Titulo = titulo,
                    Duracion = duracion,
                    Genero = genero,
                    Estado = string.IsNullOrWhiteSpace(estado) ? "activo" : estado,
                    RestriccionEdad = restriccionEdad,
                    Sipnosis = sipnosis,
                    FechaEstreno = fechaEstreno
                };

                _context.Peliculas.Add(nuevaPelicula);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Película creada correctamente", id = nuevaPelicula.IdPelicula });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPost("subirImagen")]
        public async Task<IActionResult> SubirImagen([FromForm] string titulo, IFormFile archivo1, IFormFile archivo2, [FromForm] string trailer)
        {
            if (archivo1 == null || archivo1.Length == 0)
                return BadRequest(new { mensaje = "No se ha enviado archivo para la imagen vertical" });

            if (archivo2 == null || archivo2.Length == 0)
                return BadRequest(new { mensaje = "No se ha enviado el archivo para la imagen horizontal." });

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(p => p.Titulo.ToLower() == titulo.ToLower() && p.Estado.ToLower() != "borrado");

            if (pelicula == null)
                return NotFound(new { mensaje = "Película no encontrada" });

            try
            {
                var imgV = await _cloudinaryService.SubirImagenAsync(archivo1);
                var imgH = await _cloudinaryService.SubirImagenAsync(archivo2);

                // Asignar TODOS los valores recibidos
                pelicula.ImagenVertical = imgV;
                pelicula.ImagenHorizontal = imgH;
                pelicula.Trailer = trailer;

                _context.Peliculas.Update(pelicula);
                await _context.SaveChangesAsync();

                // Puedes mejorar el mensaje de éxito para ser más claro
                return Ok(new { mensaje = "Imágenes y tráiler actualizados correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al subir los archivos", detalle = ex.Message });
            }
        }
        // Eliminar película (cambia estado a "borrado")
        [HttpDelete("eliminar")]
        public async Task<IActionResult> EliminarPelicula(string titulo)
        {
            // Verificamos que el título no venga vacío
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest(new { mensaje = "El título no puede estar vacío." });
            }

            try
            {
                // Usamos ToLower() y Trim() para que la búsqueda no distinga mayúsculas y ignore espacios.
                var pelicula = await _context.Peliculas.FirstOrDefaultAsync(p => p.Titulo.ToLower() == titulo.Trim().ToLower());

                if (pelicula == null || pelicula.Estado.ToLower() == "borrado")
                {
                    return NotFound(new { mensaje = $"No se encontró la película con el título '{titulo}' o ya fue eliminada." });
                }

                pelicula.Estado = "borrado";

                _context.Peliculas.Update(pelicula);
                await _context.SaveChangesAsync();

                // ✅ CAMBIO: Mensaje de éxito más claro para el usuario.
                return Ok(new { mensaje = $"La película '{pelicula.Titulo}' fue eliminada correctamente." });
            }
            catch (Exception ex)
            {
                // Es buena práctica registrar el error real en el servidor para depuración
                // Console.WriteLine(ex.Message);
                return StatusCode(500, new { mensaje = "Error interno del servidor." });
            }
        }
    }

}
