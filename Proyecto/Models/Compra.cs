using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing.Text;
using System.Text.Json.Serialization;
using Proyecto.Controllers;
using Proyecto.Data;

namespace Proyecto.Models
{
    public class Compra
    {
        [Key]
        public int idCompra { get; set; }

        // Relación con Cuenta
        [Required]
        public int idCuenta { get; set; }

        [ForeignKey(nameof(idCuenta))]
        [JsonIgnore]
        public Cuenta? Cuenta { get; set; } // Nulable porque no siempre se carga

        // Relación con Película
        [Required]
        public int idPelicula { get; set; } // No nulable, una compra SIEMPRE tiene una película

        [ForeignKey(nameof(idPelicula))]
        [JsonIgnore]
        public Pelicula? Pelicula { get; set; } // Nulable porque no siempre se carga

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Buena práctica para dinero
        public decimal Total { get; set; }

        // El método estático está perfecto como lo tenías
        public static Compra GenerarVenta(int idCuenta, int idPelicula, decimal total)
        {
            if (total < 0)
            {
                throw new ArgumentException("El total no puede ser negativo.");
            }

            return new Compra
            {
                idCuenta = idCuenta,
                idPelicula = idPelicula,
                Total = total,
                Fecha = DateTime.UtcNow
            };
        }

    }
}
