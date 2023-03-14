using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    [Table("TB_BitacoraElementos")]
    public class BitacoraElementos
    {
        #region Propiedades

        [Column("IdBitacoraElementos"), Key]
        public int Id { get; set; }

        [Column("FechaEntrega")]
        public DateTime FechaEntrega { get; set; }

        [Column("FechaRecibe")]
        public DateTime FechaRecibe { get; set; } 

        [Column("IdTaquilleroEntrega")]
        public int IdTaquilleroEntrega { get; set; }

        [Column("ObservacionEntrega")]
        public string ObservacionEntrega { get; set; }

        [Column("IdTaquilleroRecibe")]
        public int IdTaquilleroRecibe { get; set; }

        [Column("ObservacionRecibe")]
        public string ObservacionRecibe { get; set; }
        
        [Editable(false)]
        public IEnumerable<BitacoraElementosDetalle> BitacoraElementoDetalle { get; set; }        

        #endregion Propiedades

        #region Constructor
        #endregion Constructor

        #region Metodos
        #endregion Metodos

    }
}
