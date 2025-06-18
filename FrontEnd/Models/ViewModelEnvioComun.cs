using Compartido.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Models
{
    public class ViewModelEnvioComun
    {
        public ComunDTO comunDTO {  get; set; }
        public List<SelectListItem> Agencias { get; set; }
        public List<SelectListItem> Clientes { get; set; }
    }
}
