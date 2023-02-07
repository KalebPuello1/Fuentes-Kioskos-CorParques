using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
    public class ProductosMesaCocina
    {

        public int Id_Producto { get; set; }
        public string NombreProd { get; set; }
        public int Id_Pedido { get; set; }
        public int IdDetallePedido { get; set; }
        public int Entregado { get; set; }
        public int EstadoDetallePedido { get; set; }
        public int IdMesa { get; set; }
        public string NombreMesa { get; set; }
        public int Id_TipoProdRestaurante { get; set; }
        public string Acompanamientos { get; set; }
        public DateTime FechaCreacion { get; set; }
        public TimeSpan HoraCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public TimeSpan HoraActualizacion { get; set; }
        public string Descripcion { get; set; }
        public int OpcionEntrega { get; set; }
        

    }
}
