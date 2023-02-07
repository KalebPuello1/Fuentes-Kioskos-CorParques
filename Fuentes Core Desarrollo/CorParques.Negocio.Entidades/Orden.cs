using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class Orden
    {

        #region Propiedades

        [Column("idOrden")]
        public int IdOrden { get; set; }
        [Column("descripcion")]
        public string descripcion { get; set; }

        #endregion


    }
}
