using Proyecto.Data;

namespace Proyecto.Models
{
    public class administrador
    {
        
        public int id { get; set; }
        public async Task<bool> RolCliente(Database context, string estado)
        {
            return await CambiarEstadoRol(context, "Cliente", estado);
        }

        // Cambiar estado de rol Administrativo
        public async Task<bool> RolAdministrador(Database context, string estado)
        {
            return await CambiarEstadoRol(context, "Administrativo", estado);
        }

        // Cambiar estado de rol Soporte
        public async Task<bool> RolSoporte(Database context, string estado)
        {
            return await CambiarEstadoRol(context, "Soporte", estado);
        }

        // Cambiar estado de rol Ventas
        public async Task<bool> RolVenta(Database context, string estado)
        {
            return await CambiarEstadoRol(context, "Ventas", estado);
        }

        // Cambiar estado de rol Funciones
        public async Task<bool> RolFuncion(Database context, string estado)
        {
            return await CambiarEstadoRol(context, "Funciones", estado);
        }

        // Método privado que hace la modificación en la BD
        private async Task<bool> CambiarEstadoRol(Database context, string rol, string estado)
        {
            var cuentaBD = await context.Cuentas.FindAsync(this.id);
            if (cuentaBD == null)
                throw new Exception("Cuenta no encontrada");

            string nuevoEstado = estado.ToLower() == "activo" ? "activo" : "barrado";

            switch (rol)
            {
                case "Cliente":
                    cuentaBD.Cliente = nuevoEstado;
                    break;
                case "Administrativo":
                    cuentaBD.Administrativo = nuevoEstado;
                    break;
                case "Soporte":
                    cuentaBD.Soporte = nuevoEstado;
                    break;
                case "Ventas":
                    cuentaBD.Ventas = nuevoEstado;
                    break;
                case "Funciones":
                    cuentaBD.Funciones = nuevoEstado;
                    break;
                default:
                    throw new Exception("Rol no válido");
            }

            context.Cuentas.Update(cuentaBD);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
