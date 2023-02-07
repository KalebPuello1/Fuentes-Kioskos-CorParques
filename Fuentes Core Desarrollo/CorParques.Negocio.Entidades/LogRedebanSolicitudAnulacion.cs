using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_LogRedebanSolicitudAnulacion")]
    public class LogRedebanSolicitudAnulacion
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
        [Column("MensajeEnvio")]
        public string MensajeEnvio { get; set; }
        [Column("Operacion")]
        public string Operacion { get; set; }
        [Column("NumeroRecibo")]
        public string NumeroRecibo { get; set; }
        [Column("NumeroFactura")]
        public string NumeroFactura { get; set; }
        [Column("Clave")]
        public string Clave { get; set; }
        [Column("CodigoCajero")]
        public string CodigoCajero { get; set; }
        [Column("IP")]
        public string IP { get; set; }
        [Column("DireccionMAC")]
        public string DireccionMAC { get; set; }
        [Column("Localizacion")]
        public string Localizacion { get; set; }

        #endregion


    }
}
