using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
  public  class DetalleCortesia
    {
        public int IdDetalleCortesia { get; set; }
        public int IdProducto { get; set; }
        public int IdCortesia { get; set; }
        public string CodigoSap { get; set; }

        public string Nombre { get; set; }
        public string CodSapTipoProducto { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Observacion { get; set; }
        public bool Activo { get; set; }

        public bool Aprobacion { get; set; }

        public string DescripcionBeneficioFAN { get; set; }

        public string Consecutivo { get; set; }

        public DateTime FechaEntrega { get; set; }



    }
}
