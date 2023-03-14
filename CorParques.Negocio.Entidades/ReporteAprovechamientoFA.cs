using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class ReporteAprovechamientoFA
    {        
        public string numerofactura { get; set; }
        public string NIT { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FehcaNegociacion { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string NumeroSolicitud { get; set; }
        public int cantidadVendidas { get; set; }
        public int cantidadRedimida { get; set; }
        public int CantidadNoRedimidas { get; set; }
        public double ValorAprovechamiento { get; set; }
        public string producto { get; set; }
        public string NombreAsesor { get; set; }
        [Editable(false)]
        public string fechaInicial { get; set; }
        [Editable(false)]
        public string fechaFinal { get; set; }
        [Editable(false)]
        public string cliente { get; set; }
        [Editable(false)]
        public string pedido { get; set; }
        [Editable(false)]
        public string factura { get; set; }


    }
}
