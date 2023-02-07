using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
        
    public class PlaneacionAuxiliar
    {

        #region Propiedades

        [Editable(false)]
        public int IdPlaneacion { get; set; }

        [Editable(false)]
        public int IdIndicador { get; set; }

        [Editable(false)]
        public DateTime Fecha { get; set; }

        [Editable(false)]
        public decimal Valor { get; set; }        

        [Editable(false)]
        public string FechaTexto { get; set; }

        [Editable(false)]
        public string ValorTexto { get; set; }

        [Editable(false)]
        public DateTime FechaPlaneacion { get; set; }

        

        #endregion


    }
}
