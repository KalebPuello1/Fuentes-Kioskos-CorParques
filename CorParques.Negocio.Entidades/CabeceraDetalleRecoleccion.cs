using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{    
    public class CabeceraDetalleRecoleccion
    {

        #region Propiedades

        public int IdApertura { get; set; }
        public int IdRecoleccion { get; set; }
        public IEnumerable<DetalleRecoleccion> objDetalle { get; set; }

        #endregion


    }
}
