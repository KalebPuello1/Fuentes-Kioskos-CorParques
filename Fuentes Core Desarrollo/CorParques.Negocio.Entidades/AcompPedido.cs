using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorParques.Negocio.Entidades
{
   public class AcompPedido
    {
        
        public int IdDetallePedido { get; set; }
        public int Id_Producto { get; set; }
        public int Id_Pedido { get; set; }
        public int Id_Acomp { get; set; }
        public string Observaciones { get; set; }
        public string Nombre { get; set; }
        public int Entregado { get; set; }

        public string NombreCliente { get; set; }
    }
}
