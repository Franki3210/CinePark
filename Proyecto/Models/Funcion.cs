using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Proyecto.Models
{
    public class Funcion
    {
        [Key]
        public int id { get; set; }
        public int idSala { get; set; }
        [ForeignKey(nameof(idSala))]
        [JsonIgnore]
        public virtual Sala Sala { get; set; }  // 1 Sala puede tener muchas Funciones

        public int idPelicula { get; set; }
        [ForeignKey(nameof(idPelicula))]
        [JsonIgnore]
        public virtual Pelicula Pelicula { get; set; }

        public int Precio { get; set; }

        // Estado privado con propiedad pública para validación
        private string _Estado;
        public string Estado
        {
            get => _Estado;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El estado no puede estar vacío.");
                _Estado = value;
            }
        }

        // Fecha privada con propiedad pública para validación
        private DateOnly _Fecha;
        public DateOnly Fecha
        {
            get => _Fecha;
            set
            {
                if (value < DateOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentException("La fecha no puede ser en el pasado.");
                _Fecha = value;
            }
        }

        public TimeOnly horaInicio { get; set; }
        public TimeOnly horaFinal { get; set; }

        // Método para programar función
        public static Funcion ProgramarFuncion(int idSala, int idPelicula, DateOnly fecha, TimeOnly inicio, TimeOnly fin, int precio)
        {
            return new Funcion
            {
                idSala = idSala,
                idPelicula = idPelicula,
                Fecha = fecha,          // Se valida en la propiedad pública
                horaInicio = inicio,
                horaFinal = fin,
                Precio = precio,
                Estado = "Programada"   // Se valida en la propiedad pública
            };
        }

        // Método para modificar función
        public void ModificarFuncion(int? idSala = null, int? idPelicula = null, DateOnly? fecha = null, TimeOnly? inicio = null, TimeOnly? fin = null, int? precio = null)
        {
            if (idSala.HasValue) this.idSala = idSala.Value;
            if (idPelicula.HasValue) this.idPelicula = idPelicula.Value;
            if (fecha.HasValue) this.Fecha = fecha.Value; // Se valida en la propiedad pública
            if (inicio.HasValue) this.horaInicio = inicio.Value;
            if (fin.HasValue) this.horaFinal = fin.Value;
            if (precio.HasValue) this.Precio = precio.Value;
        }

        // Método para cancelar función
        public void CancelarFuncion()
        {
            this.Estado = "Cancelada"; // Se valida en la propiedad pública
        }

    }
}
