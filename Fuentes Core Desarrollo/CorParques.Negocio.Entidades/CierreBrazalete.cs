using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    public class CierreBrazalete
    {

        #region Propiedades

        [Editable(false)]
        public int IdTipoBrazalete { get; set; }

        [Editable(false)]
        public string TipoBrazalete { get; set; }

        [Editable(false)]
        public int Asignados { get; set; }

        [Editable(false)]
        public int TotalVendidos { get; set; }
        [Editable(false)]
        public int TotalDiferencia { get; set; }
        
        [Editable(false)]
        public int Revision { get; set; }

        [Editable(false)]
        public int EnCaja { get; set; }

        [Editable(false)]
        public int IdAperturaBrazaleteDetalle { get; set; }
        [Editable(false)]
        public string CodigoSap { get; set; }
        public string CodSapTipoProducto { get; set; }

        #endregion

    }
}
