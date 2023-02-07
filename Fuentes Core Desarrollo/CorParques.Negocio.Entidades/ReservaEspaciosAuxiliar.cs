using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
        
    public class ReservaEspaciosAuxiliar
    {

        #region Propiedades

        [Editable(false)]
        public string NombreCliente { get; set; }
        
        [Editable(false)]
        public string NombreVendedor { get; set; }

        [Editable(false)]
        public string Producto { get; set; }

        [Editable(false)]
        public string Cantidad { get; set; }

        #endregion
    }

}
