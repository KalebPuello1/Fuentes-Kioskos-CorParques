using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioCodigodeBarras : IServicioCodigodeBarras
    {
        protected IRepositorioCodigodeBarras _repositorio;


        #region Constructor
        
        public ServicioCodigodeBarras(IRepositorioCodigodeBarras CodigodeBarras)
        {
            this._repositorio = CodigodeBarras;
        }

        #endregion

        #region Metodos
         
        /// <summary>
        /// RDSH: Valida los requisitos para que un punto pueda operar.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public ConsultaCodigodeBarras ValidarPermisoOperativoPunto(int intIdPunto, out string strError)
        {
            return _repositorio.ValidarPermisoOperativoPunto(intIdPunto, out strError);
        }

        /// <summary>
        /// RDSH: Valida si un codigo de barras es valido para ingresar a una atracción o destreza.
        /// </summary>
        /// <param name="strCodigoBarras"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public ConsultaCodigodeBarras ValidarCodigoBarras(string strCodigoBarras, int intIdPunto, int intIdUsuario, long lngValorAcumulado, long lngValorAcumuladoConvenio, string strCodigoDeBarrasDescargar, out string strError)
        {
            return _repositorio.ValidarCodigoBarras(strCodigoBarras, intIdPunto, intIdUsuario, lngValorAcumulado, lngValorAcumuladoConvenio, strCodigoDeBarrasDescargar, out strError);
        }

        /// <summary>
        /// RDSH: Transfiere el dinero acumulado de varios codigos de barra a uno que haya sido leido dos veces.
        /// </summary>
        /// <param name="strCodigoACargar"></param>
        /// <param name="strCodigoDeBarrasDescargar"></param>
        /// <param name="lngValorAcumulado"></param>
        /// <param name="intIdUsuario"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public ConsultaCodigodeBarras TransferirSaldo(string strCodigoACargar, string strCodigoDeBarrasDescargar, long lngValorAcumulado, int intIdUsuario, out string strError)
        {
            return _repositorio.TransferirSaldo(strCodigoACargar, strCodigoDeBarrasDescargar, lngValorAcumulado, intIdUsuario, out strError);
        }

        #endregion

        #region Metodos no implementados

        public bool Actualizar(ConsultaCodigodeBarras modelo)
        {
            throw new NotImplementedException();
        }

        public ConsultaCodigodeBarras Crear(ConsultaCodigodeBarras modelo)
        {
            throw new NotImplementedException();
        }

        public ConsultaCodigodeBarras Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ConsultaCodigodeBarras> ObtenerTodos()
        {
            throw new NotImplementedException();
        }



        #endregion

    }
}
