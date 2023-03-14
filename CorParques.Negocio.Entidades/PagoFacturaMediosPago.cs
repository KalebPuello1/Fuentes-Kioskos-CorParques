using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class PagoFacturaMediosPago
    {
        public int IdMedioPago { get; set; }
        public string NumReferencia { get; set; }
        public double Valor { get; set; }
        public int IdFranquicia { get; set; }
        public string NombreMedioPago { get; set; }
        
    }
}
