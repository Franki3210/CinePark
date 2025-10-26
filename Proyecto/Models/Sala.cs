using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Sala
    {
        [Key]
        public int Id_sala { get; set; }
        [Required, MaxLength(10)]
        //sala-1 
        public string Nombre
        {
            get
            {
                return _Nombre;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("El nombre de la sala no debe estar vacia");
                }
                _Nombre = value;
            }
        }
        private string _Nombre;

        private int _Capacidad;
        public int Capacidad
        {
            get { return _Capacidad; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("La sala debe tener almenos 1 asiento");
                }
                _Capacidad = value;
            }
        }
        //activo o desactivado
        private string _Estado;
        [Required, MaxLength(9)]

        public string Estado
        {
            get { return _Estado; }
            set
            {
                if (value != "activo" && value != "borrado")
                {
                    throw new ArgumentException("Ah");
                }

                _Estado = value;
            }
        }
        //para la relacion con asientos 1:n
        public ICollection<Asiento> Asientos { get; set; } = new List<Asiento>();
        public void habilitarSala()
        {
            this.Estado = "activo";

        }
        public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
    }
}
