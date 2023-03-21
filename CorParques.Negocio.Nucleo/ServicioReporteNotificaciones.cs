using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioReporteNotificaciones : IServicioReporteNotificaciones
    {

        #region Declaraciones

        private readonly IRepositorioReporteNotificaciones _repositorio;

        #endregion

        #region Constructor

        public ServicioReporteNotificaciones(IRepositorioReporteNotificaciones repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        #region Metodos
        /// <summary>
        /// RDSH: retorna la informacion de las notificaciones enviadas por rango de fecha.
        /// </summary>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <returns></returns>
        public IEnumerable<ReporteNotificaciones> ObtenerReporte(string FechaInicial, string FechaFinal)
        {            
            return _repositorio.ObtenerReporte(FechaInicial, FechaFinal);
        }

        #endregion
    }
}
