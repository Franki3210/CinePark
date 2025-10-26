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
    public class administradorsController : ControllerBase
    {
        private readonly Database _context;

        public administradorsController(Database context)
        {
            _context = context;
        }

        // GET: api/administradors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<administrador>>> Getadministrador()
        {
            return await _context.administrador.ToListAsync();
        }
        [HttpPut("ModificarRol")]
        public async Task<IActionResult> ModificarRol(string correo, string rol, string estado)
        {
            try
            {
                // Buscar cuenta por correo
                var cuentaBD = await _context.Cuentas.FirstOrDefaultAsync(c => c.Email == correo);
                if (cuentaBD == null)
                    return NotFound("Cuenta no encontrada");

                var admin = new administrador { id = cuentaBD.idcuenta };

                bool resultado = rol switch
                {
                    "Cliente" => await admin.RolCliente(_context, estado),
                    "Administrativo" => await admin.RolAdministrador(_context, estado),
                    "Soporte" => await admin.RolSoporte(_context, estado),
                    "Ventas" => await admin.RolVenta(_context, estado),
                    "Funciones" => await admin.RolFuncion(_context, estado),
                    _ => throw new Exception("Rol no válido")
                };

                return Ok(new { mensaje = $"Rol {rol} actualizado a {estado}", resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private bool administradorExists(int id)
        {
            return _context.administrador.Any(e => e.id == id);
        }
    }
}
