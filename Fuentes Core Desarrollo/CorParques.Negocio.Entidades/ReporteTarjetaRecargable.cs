using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteTarjetaRecargable
    {        
        public DateTime FechaCompra { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string Cliente { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoTarjeta { get; set; }
        public string Estado { get; set; }
        public double SaldoInicial { get; set; }
        public double NuevaRecarga { get; set; }
        public double ValorUsado { get; set; }
        public double SaldoRecarga { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Factura { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

    }
}
