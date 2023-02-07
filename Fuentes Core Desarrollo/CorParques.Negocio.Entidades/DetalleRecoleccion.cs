using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{    
    public class DetalleRecoleccion
    {

        #region Propiedades

        public int IdApertura { get; set; }
        public int IdRecoleccion { get; set; }
        public string Tipo { get; set; }
        public int Total { get; set; }
        public string NumeroSobre { get; set; }
        public string TipoRecoleccion { get; set; }
        public int? IdUsuarioEntrega { get; set; }
        public int? IdUsuarioRecibe { get; set; }

        #endregion


    }
}
