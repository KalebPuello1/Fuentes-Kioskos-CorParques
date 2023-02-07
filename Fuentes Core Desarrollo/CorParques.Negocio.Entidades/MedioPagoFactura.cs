using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    public class MedioPagoFactura
    {

        #region Propiedades

        public int IdMedioPagoFactura { get; set; }

        public int IdMedioPago { get; set; }

        public string NombreMedioPago { get; set; }

        public int IdFranquicia { get; set; }

        public int Id_Factura { get; set; }

        public string NumReferencia { get; set; }

        public long Valor { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioModificacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public bool Enviado { get; set; }

        public long Cambio { get; set; }

        public string CodigoFactura { get; set; }

        public string ConsecutivoConvenio { get; set; }
        public string NumeroFactura { get; set; }
        public string NumeroTarjeta { get; set; }

        public string NumeroAprobacion { get; set; }


        #endregion


    }
}
