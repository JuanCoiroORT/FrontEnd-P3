using Compartido.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models
{
    public class ViewModelEnvioUrgente
    {
        public UrgenteDTO urgenteDTO {  get; set; }
        public List<SelectListItem> Clientes { get; set; }
    }
}
