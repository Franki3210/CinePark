using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Pelicula
    {
        [Key] //llave primaria
        public int IdPelicula { get; set; }

        private string _Titulo; // variable encapsulada
        [Required, MaxLength(100)] // controlar que no sea nulo y longitud maxima

        public string Titulo // propiedad para acceder a la variable encapsulada
        {
            get => _Titulo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El título no debe estar vacío");
                }
                _Titulo = value;
            }
        }

        private int _Duracion;
        [Required]
        public int Duracion
        {
            get => _Duracion;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("La duración debe ser mayor a 0");
                }
                _Duracion = value;
            }
        }

        private string _Genero;
        [Required, MaxLength(200)]
        public string Genero
        {
            get => _Genero;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("El género no debe estar vacío");
                }
                _Genero = value;
            }
        }

        private string _Estado = "activo";
        [Required, MaxLength(10)]

        public string Estado
        {
            get => _Estado;
            set => _Estado = string.IsNullOrWhiteSpace(value) ? "activo" : value;
        }

        private string _RestriccionEdad;
        [Required, MaxLength(100)]
        public string RestriccionEdad
        {
            get => _RestriccionEdad;
            set => _RestriccionEdad = value;
        }

        private string _Sipnosis;
        [Required, MaxLength(1000)]
        public string Sipnosis
        {
            get => _Sipnosis;
            set => _Sipnosis = value;
        }
        private DateOnly _FechaEstreno;
        [Required]
        public DateOnly FechaEstreno
        {
            get => _FechaEstreno;
            set
            {
                if (value == default)
                {
                    throw new ArgumentException("El título no debe estar vacío");
                }
                _FechaEstreno = value;
            }
        }
        private string? _imagenVertical;
        [MaxLength(500)]
        public string? ImagenVertical
        {
            get => _imagenVertical;
            set => _imagenVertical = value;
        }

        private string? _imagenHorizontal;
        [MaxLength(500)]
        public string? ImagenHorizontal
        {
            get => _imagenHorizontal;
            set => _imagenHorizontal = value;
        }

        private string? _trailer;
        [MaxLength(500)]
        public string? Trailer
        {
            get => _trailer;
            set => _trailer = value;
        }

        // Metodo para obtener datos de pelicula
        public void ObtenerDatos()
        {
            // Puedes implementar si necesitas
        }

        // Metodo para actualizar datos de pelicula
        public void actualizarDatos(string titulo, int duracion, string genero, string estado, string restriccionEdad, string sipnosis, DateOnly fechaEstreno, string? imagenVertical, string? imagenHorizontal, string? trailer)
        {
            this.Titulo = titulo;
            this.Duracion = duracion;
            this.Genero = genero;
            this.Estado = estado;
            this.RestriccionEdad = restriccionEdad;
            this.Sipnosis = sipnosis;
            this.FechaEstreno = fechaEstreno;
            this.ImagenVertical = imagenVertical;
            this.ImagenHorizontal = imagenHorizontal;
            this.Trailer = trailer;
        }
        public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    }
}
