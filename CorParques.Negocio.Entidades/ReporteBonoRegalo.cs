using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
   public class ReporteBonoRegalo
    {

        public string FechaInicialP { get; set; }
        public string FechaFinalP { get; set; }
        public int? IdTipoPedido { get; set; }
        public string CodCliente { get; set; }
        public string CodVendedor { get; set; }
        public string CodPedido { get; set; }

    

        //--------------------------------------------
        //Reporte

        //public string FechaMovimiento { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string EsBoletaControl { get; set; }
        public string Fiestas { get; set; }
        public string Adicional { get; set; }
        public string CodSapPedido { get; set; }
        public string CodSapCliente { get; set; }
        public string NombreCliente { get; set; }
        public string CodSapVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Valor { get; set; }
        public decimal saldo { get; set; }
        public decimal consumo { get; set; }


    }
}
