using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Proyecto.Models
{
    public class Cuenta
    {
        [Key]
        public int idcuenta { get; set; }
        public int IdPersona { get; set; }

        // 🔑 Relación uno a uno
        [ForeignKey(nameof(IdPersona))]
        [JsonIgnore]
        public Persona Persona { get; set; }

        private string _Email;
        [Required, MaxLength(100)]
        public string Email
        {
            get => _Email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El email no puede estar vacío");

                string normalizedEmail = value.Trim().ToLower();

                if (!normalizedEmail.EndsWith("@gmail.com"))
                    throw new ArgumentException("El email debe terminar con @gmail.com");

                _Email = normalizedEmail;
            }
        }

        private string _Contrasena;
        [Required]
        public string Contrasena
        {
            get => _Contrasena;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La contraseña no puede estar vacía.");

                if (value.Length < 8)
                    throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
                if (value.Length > 256)
                    throw new ArgumentException("La contraseña no puede tener más de 256 caracteres.");
                if (!value.Any(char.IsUpper))
                    throw new ArgumentException("La contraseña debe contener al menos una mayúscula.");
                if (!value.Any(char.IsLower))
                    throw new ArgumentException("La contraseña debe contener al menos una minúscula.");
                if (!value.Any(char.IsDigit))
                    throw new ArgumentException("La contraseña debe contener al menos un número.");

                string specialCharacters = "@$!%*?&";
                if (!value.Any(c => specialCharacters.Contains(c)))
                    throw new ArgumentException("La contraseña debe contener al menos un carácter especial (@$!%*?&).");

                _Contrasena = BCrypt.Net.BCrypt.HashPassword(value, workFactor: 12);
            }
        }
        private string _Cliente;
        [Required,MaxLength(9)]

        public string Cliente
        {
            get => _Cliente;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El campo Cliente no puede estar vacío.");

                string v = value.Trim().ToLower();
                if (v != "activo" && v != "borrado")
                    throw new ArgumentException("El campo Cliente solo puede ser 'activo' o 'borrado'.");

                _Cliente = v;
            }
        }

        private string _Administrativo;
        [Required,MaxLength(9)]

        public string Administrativo
        {
            get => _Administrativo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El campo Administrativo no puede estar vacío.");

                string v = value.Trim().ToLower();
                if (v != "activo" && v != "borrado")
                    throw new ArgumentException("El campo Administrativo solo puede ser 'activo' o 'borrado'.");

                _Administrativo = v;
            }
        }

        private string _Soporte;
        [Required,MaxLength(9)]
        public string Soporte
        {
            get => _Soporte;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El campo Soporte no puede estar vacío.");

                string v = value.Trim().ToLower();
                if (v != "activo" && v != "borrado")
                    throw new ArgumentException("El campo Soporte solo puede ser 'activo' o 'borrado'.");

                _Soporte = v;
            }
        }

        private string _Ventas;
        [Required, MaxLength(9)]
        public string Ventas
        {
            get => _Ventas;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El campo Ventas no puede estar vacío.");

                string v = value.Trim().ToLower();
                if (v != "activo" && v != "borrado")
                    throw new ArgumentException("El campo Ventas solo puede ser 'activo' o 'borrado'.");

                _Ventas = v;
            }
        }

        private string _Funciones;
        [Required,MaxLength(9)]
        public string Funciones
        {
            get => _Funciones;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El campo Funciones no puede estar vacío.");

                string v = value.Trim().ToLower();
                if (v != "activo" && v != "borrado")
                    throw new ArgumentException("El campo Funciones solo puede ser 'activo' o 'borrado'.");

                _Funciones = v;
            }
        }
        public static Cuenta CrearParaLogin(string email, string contrasena)
        {
            return new Cuenta
            {
                Email = email,       // se asigna directamente
                Contrasena = contrasena // se asigna directo; hash se genera automáticamente en el setter
            };
        }

        // Método para verificar la contraseña (ya lo tienes)
        public static bool VerificarContrasena(string contrasenaPlano, string hashGuardado)
        {
            return BCrypt.Net.BCrypt.Verify(contrasenaPlano, hashGuardado);
        }
        [JsonIgnore]
        public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}
