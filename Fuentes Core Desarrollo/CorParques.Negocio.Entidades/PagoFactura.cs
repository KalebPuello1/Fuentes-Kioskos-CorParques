using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class PagoFactura
    {
        #region Propiedades

        public List<Producto> listaProducto { get; set; }
        public List<Acompanamiento> listaAcomp { get; set; }
        public List<PagoFacturaMediosPago> listMediosPago { get; set; }
        public int IdUsuario { get; set; }
        public int IdPunto { get; set; }
        public string CodSapConvenio { get; set; }
        public int IdConvenio { get; set; }
        public string ConsecutivoConvenio { get; set; }
        public bool EsContingencia { get; set; }
        //DANR: 22-01-2019 *** Adicion de campo donante
        public string Donante { get; set; }
        public double Cambio { get; set; }

        public int IdMesa { get; set; }

        public string NombreCliente { get; set; }
        public int? IdPedido{ get; set; }
        public Int64? IdFactura { get; set; }
        //fin DANR: 22-01-2019 *** Adicion de campo donante
        public bool BanderaBonoRegalo { get; set; }
        #endregion Propiedades

        #region Metodos

        #endregion Metodos 

        #region Contructor

        public PagoFactura()
        {
            //DANR: 22-01-2019 *** Adicion de campo donante
            Donante = null;
            //fin DANR: 22-01-2019 *** Adicion de campo donante
            this.listaProducto = new List<Producto>();
            this.listMediosPago = new List<PagoFacturaMediosPago>();
        }
       

        #endregion Contructor

    }
}
