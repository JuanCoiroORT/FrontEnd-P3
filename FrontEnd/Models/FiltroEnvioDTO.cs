﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs
{
    public class FiltroEnvioDTO
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin {  get; set; }
        public string? Estado { get; set; }
    }
}
