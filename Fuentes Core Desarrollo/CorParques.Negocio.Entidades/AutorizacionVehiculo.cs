using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_AutorizacionVehiculo")]
    public class AutorizacionVehiculo
    {

        #region Propiedades

        [Key]
        [Column("IdAutorizacionVehiculo")]
        public int IdAutorizacionVehiculo { get; set; }
        [Column("IdConvenioParqueadero")]
        public int IdConvenioParqueadero { get; set; }
        [Column("IdTipoVehiculo")]
        public int IdTipoVehiculo { get; set; }
        [Column("Placa")]
        public string Placa { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }


        [Editable(false)]
        public string TipoVehiculo { get; set; }

        #endregion


    }
}
