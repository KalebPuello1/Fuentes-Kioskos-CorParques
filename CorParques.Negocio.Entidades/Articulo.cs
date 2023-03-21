using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class Articulo
    {

        #region Declaraciones

        private string strNombre;
        private int intCantidad;
        private double dblPrecio;
        private string strOtro;
        private string strGrupo;
        private bool bitBoleteria;
        
        private string strTituloColumnas;
        private string strsubGrupo;

        #endregion

        #region Constructor

        public Articulo()
        {
            strNombre = string.Empty;
            intCantidad = 0;
            dblPrecio = 0;
            strOtro = string.Empty;
            bitBoleteria = false;
        }

        #endregion

        #region Propiedades

        public string Nombre
        {
            get { return strNombre; }
            set { strNombre = value; }
        }
        
        public int Cantidad
        {
            get { return intCantidad; }
            set { intCantidad = value; }
        }

        public long NumTicket;
        public double Precio
        {
            get { return dblPrecio; }
            set { dblPrecio = value; }
        }
        
        public string Otro
        {
            get { return strOtro; }
            set { strOtro = value; }
        }

        public string Grupo
        {
            get { return strGrupo; }
            set { strGrupo = value; }
        }

        public string subGrupo
        {
            get { return strsubGrupo; }
            set { strsubGrupo = value; }
        }
        public bool Boleteria
        {
            get { return bitBoleteria; }
            set { bitBoleteria = value; }
        }

        public string TituloColumnas
        {
            get { return strTituloColumnas; }
            set { strTituloColumnas = value; }
        }
        #endregion
    }
}
