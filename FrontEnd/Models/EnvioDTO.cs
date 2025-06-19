using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public class EnvioDTO
    {
        public int Id { get; set; }
        public double Peso { get; set; }
        public string Estado { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public double NumTracking { get; set; }

        public EnvioDTO() { }
    }
}
