using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReportePasajeros
    {

        public string Fecha { get; set; }
        public string Atraccion { get; set; }
        public string TipoProducto { get; set; }
        public string Producto { get; set; }
        public int CantidadUsos { get; set; }
        public int ValorVenta { get; set; }
        public int Valor { get; set; }
        public string Convenio { get; set; }
        public int Torniquete { get; set; }
        public string Porcentaje { get; set; }
    }
}
