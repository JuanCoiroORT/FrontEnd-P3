
namespace Compartido.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CI { get; set; }
        public Email Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }

        public UsuarioDTO() { }

    }
}
