using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public class UrgenteDTO : EnvioDTO
    {
        public int DireccionPostal { get; set; }
        public bool Eficiente { get; set; }

        public UrgenteDTO() { }
    }
}
