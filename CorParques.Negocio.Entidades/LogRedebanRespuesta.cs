using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_LogRedebanRespuesta")]
    public class LogRedebanRespuesta
    {

        #region Propiedades

        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Evento")]
        public string Evento { get; set; }
        [Column("Fecha")]
        public DateTime Fecha { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("MensajeRespuesta")]
        public string MensajeRespuesta { get; set; }
        [Column("CodigoRespuesta")]
        public string CodigoRespuesta { get; set; }
        [Column("NumeroAprobacion")]
        public string NumeroAprobacion { get; set; }
        [Column("NumeroTarjeta")]
        public string NumeroTarjeta { get; set; }
        [Column("TipoTarjeta")]
        public string TipoTarjeta { get; set; }
        [Column("Franquicia")]
        public string Franquicia { get; set; }
        [Column("Monto")]
        public double Monto { get; set; }
        [Column("Iva")]
        public double Iva { get; set; }
        [Column("NumeroRecibo")]
        public string NumeroRecibo { get; set; }
        [Column("Cuotas")]
        public int Cuotas { get; set; }
        [Column("RRN")]
        public string RRN { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("Localizacion")]
        public string Localizacion { get; set; }
        [Column("DireccionMAC")]
        public string DireccionMAC { get; set; }
        [Column("NumeroFactura")]
        public string NumeroFactura { get; set; }
        [Column("FacturaAnulada")]
        public bool FacturaAnulada { get; set; }

        [Column("IdUsuarioAnulacion")]
        public int? IdUsuarioAnulacion { get; set; }
        [Column("IPAnulacion")]
        public string IPAnulacion { get; set; }
        [Column("LocalizacionAnulacion")]
        public string LocalizacionAnulacion { get; set; }
        [Column("DireccionMACAnulacion")]
        public string DireccionMACAnulacion { get; set; }
        [Column("FechaAnulacion")]
        public DateTime? FechaAnulacion { get; set; }

        [Column("ObservacionAnulacion")]
        public string ObservacionAnulacion { get; set; }
        [Column("IdSolicitud")]
        public int IdSolicitud { get; set; }

        #endregion

    }

    public class RespuestaTransaccionRedaban
    {

        #region Propiedades

        public string MensajeRespuesta { get; set; }

        public string CodigoRespuesta { get; set; }

        public string NumeroAprobacion { get; set; }

        public string NumeroTarjeta { get; set; }

        public string TipoTarjeta { get; set; }

        public string Franquicia { get; set; }

        public double Monto { get; set; }

        public string NumeroFactura { get; set; }

        public string NumeroRecibo { get; set; }

        public int IdFranquicia { get; set; }
        #endregion

    }


}
