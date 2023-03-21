using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CorParques.Negocio.Entidades
{
    
    public class Arqueo
    {
        public string Tipo { get; set; }
        public double  MontoCaja { get; set; }
        public double Recoleccion { get; set; }
        public double Total { get; set; }
        public double Base { get; set; }
        public double RecoleccionBase { get; set; }
        public double TotalCajaBase { get; set; }     
        public IEnumerable<NotaCredito> NotaCredito { get; set; }
        public IEnumerable<CierreBrazalete> Brazalete { get; set; }

    }
}
