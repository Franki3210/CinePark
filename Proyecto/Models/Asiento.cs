using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Asiento
    {
        [Key]
        public int Id_asiento { get; set; }
        public int Id_sala { get; set; } //fk de la sala
        [Required, MaxLength(5)]

        public string Fila { get; set; }   //fila del asiendo de la sala
        public int Columna { get; set; }   //columna del asiento de la sala

        private string _Estado; //estado de la sala
        [Required, MaxLength(11)]
        //maximo de longitud sera enable unnable
        //activo desactivado

        public string Estado
        {
            get
            {
                return _Estado;
            }
            set
            {
                if (value != "activo" && value != "borrado")
                {
                    throw new ArgumentException("El estado solo debe estar activado o desactivado");
                }
                _Estado = value;
            }
        }


        //navegacion a la sala, un asiento pertenece a una sala

        public Sala? Sala { get; set; }

        public int RaizCuadrada(int numero)
        {
            //saca la raiz cuadra y redondea el numero hacia arriba 
            return (int)Math.Ceiling(Math.Sqrt(numero));
        }
        public int DivisionArriba(int numero, int divisor)
        {
            //divide el numero de la raiz en flotante y redondea arriba
            return (int)Math.Ceiling((double)numero / divisor);
        }
        public List<Asiento> GuardarAsientos(int numFilas, int numColumnas, int capacidadTotal, int idSala, Sala sala)
        {
            var nuevosAsientos = new List<Asiento>();
            int contador = 0;

            for (char fila = 'A'; fila < 'A' + numFilas; fila++)
            {
                for (int col = 1; col <= numColumnas; col++)
                {
                    // Dejamos de crear asientos cuando alcanzamos la capacidad total
                    if (contador >= capacidadTotal)
                    {
                        break;
                    }

                    nuevosAsientos.Add(new Asiento
                    {
                        Id_sala = idSala,
                        Fila = fila.ToString(),
                        Columna = col,
                        Estado = "activo",
                        Sala = sala

                    });
                    contador++;
                }
            }
            return nuevosAsientos;
        }
        public virtual ICollection<Funcion> Funciones { get; set; } = new List<Funcion>();
    }
}
