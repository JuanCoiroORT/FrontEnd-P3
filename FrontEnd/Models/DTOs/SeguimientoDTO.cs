using LogicaNegocio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs
{
    public class SeguimientoDTO
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
        public Usuario Empleado { get; set; }

        public SeguimientoDTO(Seguimiento s)
        {
            Id = s.Id;
            Comentario = s.Comentario;
            Fecha = s.Fecha;
            Empleado = s.Empleado;
        }

        public SeguimientoDTO() { }

        public Seguimiento ToSeguimiento()
        {
            Seguimiento s = new Seguimiento()
            {
                Id = this.Id,
                Comentario = this.Comentario,
                Fecha = this.Fecha,
                Empleado = this.Empleado
            };
            return s;
        }
    }
}
