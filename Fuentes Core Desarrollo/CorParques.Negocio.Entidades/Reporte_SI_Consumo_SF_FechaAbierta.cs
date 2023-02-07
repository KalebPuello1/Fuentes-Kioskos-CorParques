using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class Reporte_SI_Consumo_SF_FechaAbierta
    {
        public string CodSapPedidoo { get; set; }
        public string CodBarrasBoletaControl { get; set; }
        public string Nombree { get; set; }
        public string Tot { get; set; }
        public int Valorr { get; set; }
        public string Factura { get; set; }
        public int Redencion { get; set; }
        public DateTime? FechaRedencion { get; set; }
        public DateTime? FechaIniciall { get; set; }
        public DateTime? FechaFinall { get; set; }
        public int cantBoleteriaa { get; set; }
        public int CantTicketRed { get; set; }

        //Datos Consumo fecha abierta 

        public string DESCRIPCION { get; set; }
        public string CodSapPedido { get; set; }
        public string NombreVendedor { get; set; }
        public string Nombres { get; set; }
        public string CodSapCliente { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string CodSapTipoProducto { get; set; }
        public string Nombre { get; set; }
        public int cantBoleteria { get; set; }
        public int Valor { get; set; }
        //public int cantSaldoinicial { get; set; }
        public string cantSaldoinicial { get; set; }
        //public int ValorSaldoInicial { get; set; }
        public string ValorSaldoInicial { get; set; }
        public int CantAdicion { get; set; }
        public int Adicion { get; set; }
        /*public int cantSaldoFinal { get; set; }
        public int ValorSaldoFinal { get; set; }*/
        public int CantUsos { get; set; }
        public int SaldoUsos { get; set; }
        /*public int CantSaldoSinUsar { get; set; }
        public int SaldoSinUsar { get; set; }*/
        public int cantSaldoFinal { get; set; }
        public int ValorSaldoFinal { get; set; }

        public string Observacion { get; set; }
    }
}
