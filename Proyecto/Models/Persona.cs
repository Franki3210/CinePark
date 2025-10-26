using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Proyecto.Models
{
    public class Persona
    {
        [Key]
        public int IdPersona { get; set; }

        private string _Nombre;
        [Required]
        [MaxLength(100)]
        public string Nombre
        {
            get => _Nombre;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío");
                _Nombre = value.Trim();
            }
        }

        private string _ApellidoPaterno;
        [Required]
        [MaxLength(100)]
        public string ApellidoPaterno
        {
            get => _ApellidoPaterno;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El apellido paterno no puede estar vacío");
                _ApellidoPaterno = value.Trim();
            }
        }

        private string _ApellidoMaterno;
        [Required]
        [MaxLength(100)]
        public string ApellidoMaterno
        {
            get => _ApellidoMaterno;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El apellido materno no puede estar vacio");
                _ApellidoMaterno = value?.Trim() ?? "";
            }
        }

        private string _NumeroDeDocumento;
        [Required]
        [MaxLength(10)]
        public string NumeroDeDocumento
        {
            get => _NumeroDeDocumento;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El número de documento no puede estar vacío");
                if (value.Length > 10)
                    throw new ArgumentException("El número de documento no puede tener más de 10 caracteres");
                _NumeroDeDocumento = value.Trim();
            }
        }

        private string _Telefono;
        [Required]
        [MaxLength(8)]
        public string Telefono
        {
            get => _Telefono;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string limpio = value.Trim();
                    if (!limpio.All(char.IsDigit) || limpio.Length<8)
                        throw new ArgumentException("El telefono no tiene un formato telefonico");
         

                    _Telefono = limpio;
                }
                else
                {
                    throw new ArgumentException("El teléfono no puede estar vacío");
                }
            }
        }

        private DateOnly _FechaDeNacimiento;
        [Required]
        public DateOnly FechaDeNacimiento
        {
            get => _FechaDeNacimiento;
            set
            {
                // 1. Validar que la fecha de nacimiento no sea futura
                if (value > DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new ArgumentException("La fecha de nacimiento no puede ser futura");
                }
                // 2. Validar que la persona tenga al menos 10 años
                DateOnly fechaMinima = DateOnly.FromDateTime(DateTime.Now.AddYears(-10));
                if (value > fechaMinima)
                {
                    throw new ArgumentException("La persona debe tener al menos 10 años");
                }
                // 3. Si las validaciones pasan, se asigna el valor
                _FechaDeNacimiento = value;
            }
        }

        private string _Genero;
        [Required]
        [MaxLength(9)]
        public string Genero
        {
            get => _Genero;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El género no puede estar vacío");

                string generoNormalizado = value.Trim().ToLower();
                if (generoNormalizado != "masculino" && generoNormalizado != "femenino")
                    throw new ArgumentException("El género debe ser masculino o femenino");

                _Genero = generoNormalizado;
            }
        }
        public Cuenta Cuenta { get; set; }
        //metodo para registrar 
        // Este método recibe todo, pero no guarda la cuenta, solo devuelve los datos
        public static Persona Registrar(
            string nombre, string apellidoPaterno, string apellidoMaterno,
            string numeroDocumento, string telefono,
            DateOnly fechaNacimiento, string genero,
            string email, string contrasena,
            out string outEmail, out string outContrasena)
        {
            // Retornamos los datos de Persona
            outEmail = email;
            outContrasena = contrasena;

            return new Persona
            {
                Nombre = nombre,
                ApellidoPaterno = apellidoPaterno,
                ApellidoMaterno = apellidoMaterno,
                NumeroDeDocumento = numeroDocumento,
                Telefono = telefono,
                FechaDeNacimiento = fechaNacimiento,
                Genero = genero
            };
        }
        public void ActualizarDatos(
                    string nombre,
                    string apellidoPaterno,
                    string apellidoMaterno,
                    string numeroDocumento,
                    string telefono,
                    DateOnly fechaNacimiento,
                    string genero)
        {
            this.Nombre = nombre;
            this.ApellidoPaterno = apellidoPaterno;
            this.ApellidoMaterno = apellidoMaterno;
            this.NumeroDeDocumento = numeroDocumento;
            this.Telefono = telefono;
            this.FechaDeNacimiento = fechaNacimiento;
            this.Genero = genero;
        }
        public static bool VerificarContrasena(string contrasenaPlano, string hashGuardado)
        {
            return BCrypt.Net.BCrypt.Verify(contrasenaPlano, hashGuardado);
        }


    }
}
