using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{


    public class AdicionPedidos
    {

        #region Propiedades

        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public int Posicion { get; set; }

        public string Producto { get; set; }

        public string CodigoSapPedido { get; set; }

        public bool MostrarTexto { get; set; }

        public double Valor { get; set; }

        public string CodigoSapProducto { get; set; }

        public string ConsecutivoInicial { get; set; }

        public string ConsecutivoFinal { get; set; }

        public string Consecutivo { get; set; }
        public string CodSapTipoProducto { get; set; }
        public int IdUsuario { get; set; }
        public int IdPunto { get; set; }

        //ValidacionImpresionEnLinea
        public bool AplicaImpresionLinea { get; set; }
        public bool existe { get; set; }
        
        #endregion

    }
}
