using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly Database _context;

        public PersonasController(Database context)
        {
            _context = context;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(
    string nombre,
    string apellidoPaterno,
    string apellidoMaterno,
    string numeroDocumento,
    string telefono,
    DateOnly fechaNacimiento,
    string genero,
    string email,
    string contrasena)
        {
            try
            {
                // 1️⃣ Validar duplicados
                if (_context.Personas.Any(p => p.NumeroDeDocumento == numeroDocumento || p.Telefono == telefono))
                    return BadRequest(new { error = "Ya existe una persona con ese número de documento o teléfono." });

                string emailNormalizado = email.Trim().ToLower();
                if (_context.Cuentas.Any(c => c.Email == emailNormalizado))
                    return BadRequest(new { error = "Ya existe una cuenta con ese correo electrónico." });

                // 2️⃣ Instanciar Persona
                var persona = Persona.Registrar(
                    nombre, apellidoPaterno, apellidoMaterno,
                    numeroDocumento, telefono,
                    fechaNacimiento, // ya es DateOnly
                    genero,
                    email, contrasena,
                    out email,
                    out contrasena
                );

                // Se determina el estado del rol administrativo basado en el email.
                string estadoAdministrativo = (emailNormalizado == "wilsonyucra413@gmail.com") ? "activo" : "borrado";

                var cuenta = new Cuenta
                {
                    Email = email,
                    Contrasena = contrasena,
                    Cliente = "activo", // El cliente siempre está activo.
                    Administrativo = estadoAdministrativo, // Se asigna el estado determinado.
                    Soporte = "borrado",
                    Ventas = "borrado",
                    Funciones = "borrado",
                    Persona = persona,        // mantiene la relación
                    IdPersona = persona.IdPersona // esto garantiza que la FK se guarde
                };

                persona.Cuenta = cuenta;
                _context.Personas.Add(persona);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Registro exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.IdPersona == id);
        }
    }
}
