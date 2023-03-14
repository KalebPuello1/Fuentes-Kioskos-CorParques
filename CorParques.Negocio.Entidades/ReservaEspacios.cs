using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_ReservaEspacios")]
    public class ReservaEspacios
    {

        #region Propiedades

        [Key]
        [Column("IdReservaEspacios")]
        public int IdReservaEspacios { get; set; }
        [Column("IdEspacio")]
        public int IdEspacio { get; set; }
        [Column("IdTipoReserva")]
        public int IdTipoReserva { get; set; }        
        [Column("CodigoSapPedido")]
        public string CodigoSapPedido { get; set; }
        [Column("NombrePersona")]
        public string NombrePersona { get; set; }
        [Column("FechaReserva")]
        public string FechaReserva { get; set; }
        [Column("HoraInicio")]
        public string HoraInicio { get; set; }
        [Column("HoraFin")]
        public string HoraFin { get; set; }
        [Column("Observaciones")]
        public string Observaciones { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }

        public IEnumerable<TipoGeneral> ListaTipoEspacio { get; set; }
        public IEnumerable<TipoGeneral> ListaEspacios { get; set; }
        public IEnumerable<TipoReserva> ListaTipoReserva { get; set; }
        public IEnumerable<ReservaEspaciosAuxiliar> DetallePedido { get; set; }

        [Editable(false)]
        public int IdTipoEspacio { get; set; }

        [Editable(false)]
        public string ColorTipoReserva { get; set; }

        [Editable(false)]
        public string TipoEspacio { get; set; }

        [Editable(false)]
        public string TipoReserva { get; set; }

        [Editable(false)]
        public string Espacio { get; set; }

        [Editable(false)]
        public bool Editable { get; set; }

        #endregion


    }
}
