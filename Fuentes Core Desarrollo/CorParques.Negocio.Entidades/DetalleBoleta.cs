using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Entidades
{
    public class DetalleBoleta
    {

        #region Propiedades

        public string NombreProducto { get; set; }
        public string Punto { get; set; }
        public string Taquillero { get; set; }
        public string ClienteSap { get; set; }
        public string UsuarioBloqueo { get; set; }
        public DateTime? FechaBloqueo { get; set; }
        public string PuntoBloqueo { get; set; }
        public long? Saldo { get; set; }
        public int IdBoleta { get; set; }
        public DateTime? FechaVenta { get; set; }
        public bool BoletaInvalida { get; set; }
        public IEnumerable<HistoricoBoleta> historicoUso { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string CodigoVenta { get; set; }
        public string Cliente { get; set; }
        public int? Puntos { get; set; }
        #endregion Propiedades


        #region Constructor
        #endregion Constructor

        #region Métodos
        #endregion Métodos

    }
}
