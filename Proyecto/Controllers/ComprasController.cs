using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly Database _context;

        public ComprasController(Database context)
        {
            _context = context;
        }
        // POST: api/Compra/Registrar/{idCuenta}/{idPelicula}
        // Registra una nueva compra con un total fijo de 25.
        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarCompra(int idCuenta, int idPelicula)
        {
            if (idCuenta <= 0 || idPelicula <= 0)
            {
                return BadRequest(new { message = "Los IDs de cuenta y película deben ser válidos." });
            }

            // Las validaciones se quedan, son buenas prácticas.
            var cuentaExiste = await _context.Cuentas.AnyAsync(c => c.idcuenta == idCuenta);
            if (!cuentaExiste)
            {
                return NotFound(new { message = $"La cuenta con ID {idCuenta} no existe." });
            }

            var peliculaExiste = await _context.Peliculas.AnyAsync(p => p.IdPelicula == idPelicula);
            if (!peliculaExiste)
            {
                return NotFound(new { message = $"La película con ID {idPelicula} no existe." });
            }

            try
            {
                var nuevaCompra = Compra.GenerarVenta(idCuenta, idPelicula, 25.00m);
                _context.Compras.Add(nuevaCompra);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Compra registrada exitosamente.", data = nuevaCompra });
            }
            catch (Exception ex)
            {
                // --- ¡CAMBIO CLAVE PARA DEPURAR! ---
                // Ahora devolveremos el mensaje de error real y el "InnerException",
                // que a menudo contiene el error específico de la base de datos.
                return StatusCode(500, new
                {
                    message = "Se ha producido un error en la base de datos.",
                    error = ex.Message,
                    innerError = ex.InnerException?.Message // El '?' evita un error si InnerException es nulo.
                });
            }
        }

        // El método de Historial se queda igual.
        [HttpGet("Historial")]
        public async Task<IActionResult> ObtenerHistorialPorCuenta(int idCuenta)

        {
            if (idCuenta <= 0)
            {
                return BadRequest(new { message = "El ID de la cuenta debe ser válido." });
            }

            var historial = await _context.Compras
                .Include(compra => compra.Pelicula)
                .Where(compra => compra.idCuenta == idCuenta)
                .OrderByDescending(compra => compra.Fecha)
                .ToListAsync();

            if (!historial.Any())
            {
                return NotFound(new { message = $"No se encontró historial de compras para la cuenta con ID {idCuenta}." });
            }

            return Ok(historial);
        }
    }
}
