namespace CRUD_CORE.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public int Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Clave { get; set; }
        public string? ConfirmarClave { get; set; }

    }
}
