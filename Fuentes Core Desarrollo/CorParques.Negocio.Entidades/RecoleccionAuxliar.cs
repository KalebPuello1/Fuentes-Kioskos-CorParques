using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
        
    public class RecoleccionAuxliar
    {

        #region Propiedades

        [Editable(false)]
        public int MaximoBase { get; set; }

        [Editable(false)]
        public int MaximoCorte { get; set; }

        [Editable(false)]
        public bool MostrarBase { get; set; }

        [Editable(false)]
        public bool MostrarCorte { get; set; }

        [Editable(false)]
        public int TotalVentasDia { get; set; }

        [Editable(false)]
        public int ValorCortesRealizados { get; set; }

        [Editable(false)]
        public int MaximoCierre { get; set; }

        [Editable(false)]
        public bool ExisteCierre { get; set; }

        #endregion


    }
}
