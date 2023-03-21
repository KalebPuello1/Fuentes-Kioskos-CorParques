using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReimpresionReciboCajaV
    {
        public string Id_Factura { get; set; }
        public DateTime Fecha { get; set; }
        public int IdPunto { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdEstado { get; set; }
        public string CodigoFactura { get; set; }
        public int Contingencia { get; set; }

    }
}
