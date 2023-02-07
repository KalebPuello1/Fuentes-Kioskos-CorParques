using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{


    public class DashBoard
    {

        #region Propiedades

        [Editable(false)]
        public double Total { get; set; }

        [Editable(false)]
        public float Porcentaje { get; set; }

        [Editable(false)]
        public double Planeado { get; set; }

        [Editable(false)]
        public int IdIndicador { get; set; }

        [Editable(false)]
        public string Producto { get; set; }

        [Editable(false)]
        public int Cantidad { get; set; }

        [Editable(false)]
        public bool MostrarPresupuestoPorCumplir { get; set; }

        [Editable(false)]
        public int Numero { get; set; }

        [Editable(false)]
        public string Semaforo { get; set; }

        [Editable(false)]
        public double PresupuestoPorCumplirCalculo { get; set; }

        #endregion


    }
}
