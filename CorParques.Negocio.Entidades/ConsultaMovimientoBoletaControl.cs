using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
   public  class ConsultaMovimientoBoletaControl
    {
        public string NombreCliente { get; set; }
        public string CodSapPedido { get; set; }
        public int EstadoPedido { get; set; }
        public int EstadoBC { get; set; }
        public string FechaAbierta { get; set; }
        public string TipoInstitucional { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string CodSapVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public string Correo { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaRedencion { get; set; }
    }
}
