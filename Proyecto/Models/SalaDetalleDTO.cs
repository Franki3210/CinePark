namespace Proyecto.Models
{
    public class SalaDetalleDTO
    {
        //esta clase se usa para mostrar tanto las salas como sus asientos que pertenecen a esta
            public int Id_sala { get; set; }
            public string Nombre { get; set; }
            public int Capacidad { get; set; }
            public string Estado { get; set; }
            public List<AsientoSimpleDTO> Asientos { get; set; }
        
        //clase usada para establecerlos asientos a la hora de hacer el get por id de la sala
    }
    public class AsientoSimpleDTO
    {
        // Solo incluye las propiedades que quieres exponer
        public int Id_asiento { get; set; }
        public string Fila { get; set; }
        public int Columna { get; set; }
        public string Estado { get; set; }
    }
    public class SalaListDTO
    {
        public int Id_sala { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; }
        // NOTA: No incluimos la lista 'Asientos' aquí.
    }
    public class CrearsalaDTO
    {
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; }

    }
}
