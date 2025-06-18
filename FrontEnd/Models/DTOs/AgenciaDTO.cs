using LogicaNegocio.EntidadesNegocio;
using LogicaNegocio.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs
{
    public class AgenciaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int DireccionPostal { get; set; }
        public double Latitud {  get; set; }
        public double Longitud { get; set; }

        public AgenciaDTO() { }
        public AgenciaDTO(Agencia agencia)
        {
            Id = agencia.Id;
            Nombre = agencia.Nombre;
            DireccionPostal = agencia.DireccionPostal;
            Latitud = agencia.Ubicacion.Latitud;
            Longitud = agencia.Ubicacion.Longitud;
        }


        public Agencia ToAgencia()
        {
            Agencia agencia = new Agencia()
            {
                Id = this.Id,
                Nombre = this.Nombre,
                DireccionPostal = this.DireccionPostal,
                Ubicacion = new Ubicacion(this.Latitud, this.Longitud)
            };
            return agencia;
        }
    }
}
