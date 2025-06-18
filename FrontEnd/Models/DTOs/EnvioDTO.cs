using LogicaNegocio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs
{
    public class EnvioDTO
    {
        public int Id { get; set; }
        public double Peso { get; set; }
        public string Estado { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public double NumTracking { get; set; }

        public List<Seguimiento> Seguimientos { get; set; } = new();

        public EnvioDTO(Envio envio)
        {
            Id = envio.Id;
            Peso = envio.Peso;
            NumTracking = envio.NumTracking;
            EmpleadoId = envio.EmpleadoId;
            ClienteId = envio.ClienteId;
            Estado = envio.Estado;
            Seguimientos = envio.Seguimientos;
        }

        public EnvioDTO() { }
    }
}
