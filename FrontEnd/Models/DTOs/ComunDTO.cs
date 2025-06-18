using LogicaNegocio.EntidadesNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compartido.DTOs
{
    public class ComunDTO : EnvioDTO
    {
        public Agencia Agencia { get; set; }
        public int AgenciaId { get; set; }

        public ComunDTO(Comun envioComun)
        { 
            Id = envioComun.Id;
            Peso = envioComun.Peso;
            NumTracking = envioComun.NumTracking;
            EmpleadoId = envioComun.EmpleadoId;
            ClienteId = envioComun.ClienteId;
            Estado = envioComun.Estado;
            AgenciaId = envioComun.AgenciaId;
            Agencia = envioComun.Agencia;
            Seguimientos = envioComun.Seguimientos;
        }

        public ComunDTO() { }

        public Comun ToComun()
        {
            Comun envioComun = new Comun()
            {
                Id = this.Id,
                Peso = this.Peso,
                NumTracking = this.NumTracking,
                EmpleadoId = this.EmpleadoId,
                ClienteId = this.ClienteId,
                Estado = this.Estado,
                Seguimientos = this.Seguimientos,
                AgenciaId = this.AgenciaId,
                Agencia = this.Agencia,
            };
            return envioComun;
        }

    }
}
