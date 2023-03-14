using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CorParques.Negocio.Entidades
{
    public class TicketImprimir
    {

        #region Declaraciones   

        private string strUsuario = string.Empty;
        private string strNombrePunto = string.Empty;
        private string strTituloColumnas = string.Empty;
        private IList<Articulo> objListaArticulos;
        private string strCodigoBarras = string.Empty;
        private string strTituloTicket = string.Empty;
        private string strPieDePagina = string.Empty;

        private int idInterno = 0;
        private DataTable objTablaDetalle;
        private string strColumnaTotalizar = string.Empty;
        private string strColumnasMoneda = string.Empty;
        private IList<TicketImprimir> objListaTickets;
        private bool blnEsDataTable;
        private string strFirma = string.Empty;
        private string strAdicionarContenidoHeader = string.Empty; 

        #endregion

        #region Propiedades     

        public string Usuario
        {
            get { return strUsuario; }
            set { strUsuario = value; }
        }

        public string NombrePunto
        {
            get { return strNombrePunto; }
            set { strNombrePunto = value; }
        }


        public string TituloColumnas
        {
            get { return strTituloColumnas; }
            set { strTituloColumnas = value; }
        }

        public IList<Articulo> ListaArticulos
        {
            get { return objListaArticulos; }
            set { objListaArticulos = value; }
        }

        public string CodigoBarrasProp
        {
            get { return strCodigoBarras; }
            set { strCodigoBarras = value; }
        }

        public string TituloRecibo
        {
            get { return strTituloTicket; }
            set { strTituloTicket = value; }
        }

        public string PieDePagina
        {
            get { return strPieDePagina; }
            set { strPieDePagina = value; }
        }

        public DataTable TablaDetalle
        {
            get { return objTablaDetalle; }
            set { objTablaDetalle = value; }
        }

        /// <summary>
        /// RDSH: Totaliza por el nombre de esta columna.
        /// </summary>
        public string ColumnaTotalizar
        {
            get { return strColumnaTotalizar; }
            set { strColumnaTotalizar = value; }
        }

        /// <summary>
        /// RDSH: Recibe las columnas que se deben tener formato moneda.
        /// </summary>
        public string ColumnasMoneda
        {
            get { return strColumnasMoneda; }
            set { strColumnasMoneda = value; }
        }

        /// <summary>
        /// RDSH: Se crea esta propiedad para manejar una coleccion de ticket para que todos salgan en el mismo recibo en una sola impresión.
        /// </summary>
        public IList<TicketImprimir> ListaTickets
        {
            get { return objListaTickets; }
            set { objListaTickets = value; }
        }

        /// <summary>
        /// RDSH: Indica si el ticket usa un data table para su procesamiento.
        /// </summary>
        public bool EsDataTable
        {
            get { return blnEsDataTable; }
            set { blnEsDataTable = value; }
        }

        /// <summary>
        /// RDSH: Para enviar los nombres de las personas que firman.
        /// </summary>
        public string Firma
        {
            get { return strFirma; }
            set { strFirma = value; }
        }

        /// <summary>
        /// GALD1: Para Adicionar titulo de documento o Factura.
        /// </summary>
        public string AdicionarContenidoHeader
        {
            get { return strAdicionarContenidoHeader; }
            set { strAdicionarContenidoHeader = value; }
        }

        public int IdInterno { get => idInterno; set => idInterno = value; }


        #endregion

    }
}
