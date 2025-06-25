namespace FrontEnd.Models
{
    public class UsuarioEditPasswordDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CI { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string PasswordActual { get; set; }
        public string PasswordNueva { get; set; }
        public string ConfirmarNuevaPassword { get; set; }
    }
}
