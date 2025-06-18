using Compartido.DTOs;

namespace MVC.Models
{
    public class ViewModelEnviosIndex
    {
        public ViewModelEnviosIndex(IEnumerable<UrgenteDTO> urgentesDTO, IEnumerable<ComunDTO> comunesDTO)
        {
            UrgentesDTO = urgentesDTO;
            ComunesDTO = comunesDTO;
        }

        public IEnumerable<UrgenteDTO> UrgentesDTO { get; set; }
        public IEnumerable<ComunDTO> ComunesDTO { get; set; }
    }
}
