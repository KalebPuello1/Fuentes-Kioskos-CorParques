using Corparques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System.Collections.Generic;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporteVentasPorConvenio: IServicioReporteVentasPorConvenio
    {
        #region Declaraciones

        private readonly IRepositorioReporteVentasPorConvenio _repositorio;

        #endregion Declaraciones

        #region Constructor

        public ServicioReporteVentasPorConvenio(IRepositorioReporteVentasPorConvenio repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion Constructor

        #region Metodos

        /// <summary>
        /// RDSH: retorna la informacion de las notificaciones enviadas por rango de fecha.
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <returns></returns>
        public IEnumerable<ReporteVentasPorConvenio> ObtenerReporte(string FechaInicial, string FechaFinal)
        {
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal);
        }

        #endregion Metodos
    }
}
