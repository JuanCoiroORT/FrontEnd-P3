using Compartido.DTOs;

namespace MVC.Models
{
    public class ViewModelUsuariosIndex
    {
        public ViewModelUsuariosIndex(IEnumerable<UsuarioDTO> usuariosDTO)
        {
            this.usuariosDTO = usuariosDTO;
        }

        public IEnumerable<UsuarioDTO> usuariosDTO { get; set; }
    }
}
