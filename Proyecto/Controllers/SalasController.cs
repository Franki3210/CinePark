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
    public class SalasController : ControllerBase
    {
        private readonly Database _context;

        public SalasController(Database context)
        {
            _context = context;
        }

        // GET: api/Asientos/5
        [HttpGet("byhorario/{idHorario}")]

        //-------------------este metodo no va aun------------------------------
        public async Task<ActionResult<Asiento>> GetAsientoDisponible(int idHorario)
        {
            
            return Ok();
        }

       


        [HttpPost]
        public async Task<ActionResult<Asiento>> IngresarAsiento(int idSala)
        {
            var asiento = new Asiento();
            // Usamos FindAsync ya que solo necesitamos el objeto Sala
            var sala = await _context.Salas.FindAsync(idSala);

            if (sala == null)
            {
                return NotFound($"La Sala con ID {idSala} no existe.");
            }

            int capacidadTotal = sala.Capacidad;

            //con el calculo sacamos la raiz cuadrada de la capacidad de la sala y el resultado se vuelve a dividir por la
            //sala
            int numColumnas = asiento.RaizCuadrada(capacidadTotal);

            int numFilas = asiento.DivisionArriba(capacidadTotal, numColumnas);

            // Generación de Asientos**
            var nuevosAsientos = asiento.GuardarAsientos(numColumnas, numFilas, capacidadTotal, idSala, sala);
            // Guardar y Respuesta
            _context.Asientos.AddRange(nuevosAsientos);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = $"Se crearon {nuevosAsientos.Count} asientos asignados a la Sala ID {idSala}.",
                Distribucion = $"{numFilas} Filas x {numColumnas} Columnas"
            });


        }
        private bool AsientoExists(int id)
        {
            return _context.Asientos.Any(e => e.Id_asiento == id);
        }
        private bool SalaExists(int id)
        {
            return _context.Salas.Any(e => e.Id_sala == id);
        }
    }
}
