using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ProductosPedidos
    {
        public int IdBoleteria { get; set; }
        public string CodigoVenta { get; set; }
        public string Nombre { get; set; }
        public string CodigoSap { get; set; }
        public int IdProducto { get; set; }
        public int IdSolicitudBoleteria { get; set; }
        public int Cantidad { get; set; }
        public string NombreCliente { get; set; }
        public string Posicion { get; set; }
        public DateTime? FechaInicial { get; set; }
        public List<Puntos> ListaPuntos { get; set; }
        public List<TipoGeneral> ListaPedidos { get; set; }


    }
}
