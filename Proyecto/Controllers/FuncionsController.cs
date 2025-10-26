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
    public class FuncionsController : ControllerBase
    {
        private readonly Database _context;

        public FuncionsController(Database context)
        {
            _context = context;
        }
        [HttpPost("Crear")]
        public async Task<IActionResult> CrearFuncion(
        string nombreSala,
        string nombrePelicula,
        string fechaStr,
        string horaInicioStr,
        string horaFinalStr,
        int precio)
        {
            // Parsear fecha y horas
            if (!DateOnly.TryParse(fechaStr, out DateOnly fecha))
                return BadRequest("Formato de fecha inválido. Use yyyy-MM-dd");

            if (!TimeOnly.TryParse(horaInicioStr, out TimeOnly horaInicio))
                return BadRequest("Formato de horaInicio inválido. Use HH:mm");

            if (!TimeOnly.TryParse(horaFinalStr, out TimeOnly horaFinal))
                return BadRequest("Formato de horaFinal inválido. Use HH:mm");

            // Buscar Sala por nombre
            var sala = await _context.Salas.FirstOrDefaultAsync(s => s.Nombre == nombreSala);
            if (sala == null)
                return NotFound($"No existe la sala con nombre '{nombreSala}'");

            // Buscar Pelicula por nombre
            var pelicula = await _context.Peliculas.FirstOrDefaultAsync(p => p.Titulo == nombrePelicula);
            if (pelicula == null)
                return NotFound($"No existe la película con nombre '{nombrePelicula}'");

            try
            {
                // Usar los Ids correctos de Sala y Pelicula
                var funcion = Funcion.ProgramarFuncion(
                sala.Id_sala,      // coincide con la clase Sala
                pelicula.IdPelicula,// coincide con la clase Pelicula
                fecha,
                horaInicio,
                horaFinal,
                precio
            );

                _context.Funcion.Add(funcion);
                await _context.SaveChangesAsync();

                return Ok(funcion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Modificar")]
        public async Task<IActionResult> ModificarFuncion(
        int id,
        string nombreSala = null,
        string nombrePelicula = null,
        string fechaStr = null,
        string horaInicioStr = null,
        string horaFinalStr = null,
        int? precio = null,
        string estado = null)  // nuevo parámetro opcional
        {
            // Buscar función existente
            var funcion = await _context.Funcion
                .Include(f => f.Sala)
                .Include(f => f.Pelicula)
                .FirstOrDefaultAsync(f => f.id == id);

            if (funcion == null)
                return NotFound($"No existe la función con id {id}");

            // Buscar Id de Sala si se pasó nombre
            int? idSala = null;
            if (!string.IsNullOrWhiteSpace(nombreSala))
            {
                var sala = await _context.Salas.FirstOrDefaultAsync(s => s.Nombre == nombreSala);
                if (sala == null)
                    return NotFound($"No existe la sala con nombre '{nombreSala}'");
                idSala = sala.Id_sala;
            }

            // Buscar Id de Pelicula si se pasó nombre
            int? idPelicula = null;
            if (!string.IsNullOrWhiteSpace(nombrePelicula))
            {
                var pelicula = await _context.Peliculas.FirstOrDefaultAsync(p => p.Titulo == nombrePelicula);
                if (pelicula == null)
                    return NotFound($"No existe la película con nombre '{nombrePelicula}'");
                idPelicula = pelicula.IdPelicula;
            }

            // Parsear fecha y horas
            DateOnly? fecha = null;
            if (!string.IsNullOrWhiteSpace(fechaStr))
            {
                if (!DateOnly.TryParse(fechaStr, out DateOnly f))
                    return BadRequest("Formato de fecha inválido. Use yyyy-MM-dd");
                fecha = f;
            }

            TimeOnly? horaInicio = null;
            if (!string.IsNullOrWhiteSpace(horaInicioStr))
            {
                if (!TimeOnly.TryParse(horaInicioStr, out TimeOnly hi))
                    return BadRequest("Formato de horaInicio inválido. Use HH:mm");
                horaInicio = hi;
            }

            TimeOnly? horaFinal = null;
            if (!string.IsNullOrWhiteSpace(horaFinalStr))
            {
                if (!TimeOnly.TryParse(horaFinalStr, out TimeOnly hf))
                    return BadRequest("Formato de horaFinal inválido. Use HH:mm");
                horaFinal = hf;
            }

            // Modificar función usando tu método
            try
            {
                funcion.ModificarFuncion(idSala, idPelicula, fecha, horaInicio, horaFinal, precio);

                // Actualizar estado si se pasó
                if (!string.IsNullOrWhiteSpace(estado))
                {
                    funcion.Estado = estado; // valida automáticamente en la propiedad pública
                }

                _context.Funcion.Update(funcion);
                await _context.SaveChangesAsync();

                return Ok(funcion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Cancelar")]
        public async Task<IActionResult> CancelarFuncion(int id)
        {
            // Buscar la función existente
            var funcion = await _context.Funcion
                .FirstOrDefaultAsync(f => f.id == id);

            if (funcion == null)
                return NotFound($"No existe la función con id {id}");

            try
            {
                // Usar el método de la clase para cambiar el estado
                funcion.CancelarFuncion();

                // Guardar cambios en la base de datos
                _context.Funcion.Update(funcion);
                await _context.SaveChangesAsync();

                return Ok(funcion); // Devolver la función actualizada
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool FuncionExists(int id)
        {
            return _context.Funcion.Any(e => e.id == id);
        }
    }
}
