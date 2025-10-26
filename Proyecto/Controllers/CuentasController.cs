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
    public class CuentasController : ControllerBase
    {
        private readonly Database _context;

        public CuentasController(Database context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(string email, string contrasena)
        {
            // 1️⃣ Buscar la cuenta por email
            var cuentaBD = _context.Cuentas.FirstOrDefault(c => c.Email == email.Trim().ToLower());
            if (cuentaBD == null)
                return BadRequest(new { error = "Email o contraseña incorrectos." });

            // 2️⃣ Verificar la contraseña
            bool esValida = Cuenta.VerificarContrasena(contrasena, cuentaBD.Contrasena);
            if (!esValida)
                return BadRequest(new { error = "Email o contraseña incorrectos." });

            // 3️⃣ Preparar roles tal como están en la DB
            var roles = new
            {
                Cliente = cuentaBD.Cliente == "activo",
                Administrativo = cuentaBD.Administrativo == "activo",
                Soporte = cuentaBD.Soporte == "activo",
                Ventas = cuentaBD.Ventas == "activo",
                Funciones = cuentaBD.Funciones == "activo"
            };

            // 4️⃣ Validar si todos los roles están borrados
            if (!roles.Cliente && !roles.Administrativo && !roles.Soporte && !roles.Ventas && !roles.Funciones)
            {
                return BadRequest(new { error = "Esta cuenta no tiene acceso al sistema." });
            }

            // 5️⃣ Devolver la respuesta CORRECTA
            return Ok(new
            {
                idCuenta = cuentaBD.idcuenta, // ✅ ¡AQUÍ ESTÁ LA LÍNEA AÑADIDA!
                mensaje = "Login exitoso",
                roles
            });
        }

    }
}
