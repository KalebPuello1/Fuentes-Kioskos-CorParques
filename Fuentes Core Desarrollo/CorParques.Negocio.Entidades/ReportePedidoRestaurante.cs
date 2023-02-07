using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
   public class ReportePedidoRestaurante
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public int Id_Vendedor { get; set; }

        public int IdZona { get; set; }
        public int Id_Mesa { get; set; }
        public int IdEstadoMesa { get; set; }
        public string CodSapAlmacenOrigen { get; set; }

        //--------------------------------------------
        //Reporte

        //public string FechaMovimiento { get; set; }
        public DateTime FechaPedido { get; set; }
        public TimeSpan? HoraRegistro { get; set; }
        public TimeSpan HoraFinal{ get; set; }
        public string NombreZona { get; set; }
        public string Punto { get; set; }
        public string Almacen { get; set; }
        public string NombreMesa { get; set; }
        public string NombreVen { get; set; }
        public string ApellidoVen { get; set; }
        public int IdEstadoPedido { get; set; }
        public string NombreEstadoPedido { get; set; }

        public string Productos { get; set; }
        public decimal VentaTotal { get; set; }

        public int Id_Factura { get; set; }

    }
}
