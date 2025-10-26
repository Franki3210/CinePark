using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

namespace Proyecto.Data
{
    public class Database: DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options)
        {
            
        }
        public DbSet<Persona> Personas { get; set; } 
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Asiento> Asientos { get; set; }
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Compra> Compras { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Persona>()
                .HasOne(p => p.Cuenta)
                .WithOne(c => c.Persona)
                .HasForeignKey<Cuenta>(c => c.IdPersona);
        }
        public DbSet<Proyecto.Models.Funcion> Funcion { get; set; } = default!;
        public DbSet<Proyecto.Models.administrador> administrador { get; set; } = default!;
    }
}
