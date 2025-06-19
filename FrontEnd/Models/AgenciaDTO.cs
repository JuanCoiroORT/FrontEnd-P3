using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public class AgenciaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int DireccionPostal { get; set; }
        public double Latitud {  get; set; }
        public double Longitud { get; set; }

        public AgenciaDTO() { }


    }
}
