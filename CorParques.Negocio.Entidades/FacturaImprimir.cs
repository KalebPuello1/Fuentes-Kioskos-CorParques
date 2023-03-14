using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class FacturaImprimir
    {
        public FacturaImprimir()
        {
            ListaProductos = new List<string[]>();
            MetodosPago = new List<string[]>();
            Impuestos = new List<string[]>();
            Propina = new List<string[]>();
            BoleteriaAdicional = new List<string[]>();
            ListaProdSap = new List<string>();
        }

        public string TextoHead1 { get; set; }

        public string Id_Factura { get; set; }
        public string CodigoFactura { get; set; }
        public string IdentificacionCliente { get; set; }
        public string NombreCliente { get; set; }

        public string ConsecutivoNotaCredito { get; set; }

        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Punto { get; set; }
        public string ConsecutivoPunto { get; set; }
        public string Supervisor { get; set; }
        public List<string> ListaProdSap { get; set; }

        public List<string[]> ListaProductos { get; set; }

        public List<string[]> MetodosPago { get; set; }

        public List<string[]> Impuestos { get; set; }

        public List<string[]> Propina { get; set; }

        public string TextoFoot1 { get; set; }
        public string TextoFoot2 { get; set; }
        public string TextoFoot3 { get; set; }
        public string TextoFoot4 { get; set; }
        public string TextoFootFinal { get; set; }
        public string PuntosTextoPropinas { get; set; }
        
        public List<string[]> BoleteriaAdicional { get; set; }

        public string ResolucionPunto { get; set; }
        public DateTime FechaResolucion { get; set; }
        public string ConsecutivoInicialPunto { get; set; }
        public string ConsecutivoFinalPunto { get; set; }
        public DateTime FechaFinalResolucion { get; set; }

       

        public bool BanderaBonoRegalo { get; set; }

    }
}
