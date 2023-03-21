using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{

    [Table("TB_Recoleccion")]
    public class Recoleccion
    {

        #region Propiedades

        [Key]
        [Column("IdRecoleccion")]
        public int IdRecoleccion { get; set; }
        [Column("IdPunto")]
        public int IdPunto { get; set; }
        [Column("IdEstado")]
        public int IdEstado { get; set; }
        [Column("IdUsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }
        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [Column("IdUsuarioModificacion")]
        public int IdUsuarioModificacion { get; set; }
        [Column("FechaModificacion")]
        public DateTime FechaModificacion { get; set; }
        [Column("Cierre")]
        public bool Cierre { get; set; }
        [Column("IdUsuarioSupervisor")]
        public int IdUsuarioSupervisor { get; set; }
        [Column("IdUsuarioNido")]
        public int IdUsuarioNido { get; set; }
        [Column("IdApertura")]
        public int IdApertura { get; set; }

        public IEnumerable<DetalleRecoleccionMonetaria> RecoleccionBase { get; set; }

        public IEnumerable<DetalleRecoleccionMonetaria> RecoleccionCorte { get; set; }

        public IEnumerable<DetalleRecoleccionDocumento> RecoleccionVoucher { get; set; }

        public IEnumerable<DetalleRecoleccionDocumento> RecoleccionDocumentos { get; set; }

        public IEnumerable<TipoDenominacion> TipoDenominacion { get; set; }

        public IEnumerable<MediosPagoFactura> DocumentosRecoleccion { get; set; }

        public RecoleccionAuxliar objRecoleccionAuxliar { get; set; }

        public IEnumerable<CierreBrazalete> CierreBrazalete { get; set; }

        public IEnumerable<DetalleRecoleccionBrazalete> RecoleccionBrazalete { get; set; }
        public ObservacionRecoleccion objObservaciones { get; set; }

        public IEnumerable<DetalleRecoleccionNovedad> RecoleccionNovedad { get; set; }

        /// RDSH: Retorna las novedades pendientes de recolección.
        public IEnumerable<NovedadArqueo> NovedadesArqueo { get; set; }

        [Editable(false)]
        public string SobreMonedasBase { get; set; }
        [Editable(false)]
        public string SobreBilletesBase { get; set; }
        [Editable(false)]
        public string SobreMonedasCorte { get; set; }
        [Editable(false)]
        public string SobreBilletesCorte { get; set; }
        [Editable(false)]
        public string SobreVoucher { get; set; }
        [Editable(false)]
        public string SobreDocumentos { get; set; }
        [Editable(false)]
        public string SobreNovedad { get; set; }

        [Editable(false)]
        public string Observaciones { get; set; }

        public int cierreRecoleccion { get; set; }

        /// <summary>
        /// RDSH: Se usa para saber cual es el punto al que se le esta haciendo cierre.
        /// Esto es para el traslado de boleteria, este punto de origen es el que se selecciona en el drop down list de cierre nido.
        /// </summary>
        [Editable(false)]
        public int IdPuntoCierre { get; set; }

        [Editable(false)]
        public string ValorRecoleccionBase { get; set; }

        [Editable(false)]
        public string ValorRecoleccionCorte { get; set; }


        #endregion


    }
}
