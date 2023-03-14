using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{

    public class ServicioDashBoard : IServicioDashBoard
    {

        private readonly IRepositorioDashBoard _repositorio;

        #region Constructor

        public ServicioDashBoard(IRepositorioDashBoard repositorio)
        {
            _repositorio = repositorio;
        }



        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Retorna valores para dashboard
        /// </summary>
        /// <param name="datFechaInicial"></param>
        /// <param name="datFechaFinal"></param>
        /// <returns></returns>
        public IEnumerable<DashBoard> ObtenerInformacionDashBoard(string strFechaInicial, string strFechaFinal)
        {
            DateTime datFechaInicial;
            DateTime datFechaFinal;
            IEnumerable<DashBoard> objListaDashBoard = null;

            try
            {
                datFechaInicial = Utilidades.FormatoFechaSQLHora(strFechaInicial, "00:00");
                datFechaFinal = Utilidades.FormatoFechaSQLHora(strFechaFinal, "23:59");
                objListaDashBoard = _repositorio.ObtenerInformacionDashBoard(datFechaInicial, datFechaFinal);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "ServicioDashBoard_ObtenerInformacionDashBoard");
            }

            return objListaDashBoard;


        }

        #endregion

        #region Metodos no implementados


        public bool Actualizar(DashBoard modelo)
        {
            throw new NotImplementedException();
        }

        public DashBoard Crear(DashBoard modelo)
        {
            throw new NotImplementedException();
        }

        public DashBoard Obtener(int id)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<DashBoard> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
