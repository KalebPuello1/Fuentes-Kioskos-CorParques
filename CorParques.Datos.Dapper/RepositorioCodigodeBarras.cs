using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioCodigodeBarras : RepositorioBase<ConsultaCodigodeBarras>, IRepositorioCodigodeBarras
    {

        #region Metodos

        /// <summary>
        /// RDSH: Valida los requisitos para que un punto pueda operar.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public ConsultaCodigodeBarras ValidarPermisoOperativoPunto(int intIdPunto, out string strError)
        {

            strError = string.Empty;
            ConsultaCodigodeBarras objConsultaCodigodeBarras = null;

            try
            {

                objConsultaCodigodeBarras = _cnn.Query<ConsultaCodigodeBarras>("SP_ValidarPermisoOperativoPunto", param: new
                {
                    IdPunto = intIdPunto
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                strError = string.Concat("Error en ValidarPermisoOperativoPunto_RepositorioCodigodeBarras: ", ex.Message);
            }

            return objConsultaCodigodeBarras;
        }

        /// <summary>
        /// RDSH: Valida si un codigo de barras es valido para ingresar a una atracción o destreza.
        /// </summary>
        /// <param name="strCodigoBarras"></param>
        /// <param name="strError"></param>
        /// <returns></returns>
        public ConsultaCodigodeBarras ValidarCodigoBarras(string strCodigoBarras, int intIdPunto, int intIdUsuario, long lngValorAcumulado, long lngValorAcumuladoConvenio, string strCodigoDeBarrasDescargar, out string strError)
        {      
            ConsultaCodigodeBarras objConsultaCodigodeBarras = null;
            strError = string.Empty;

            try
            {

                objConsultaCodigodeBarras = _cnn.Query<ConsultaCodigodeBarras>("SP_ValidarAccesoCodigoBarras", param: new
                {
                    CodigoBarras = strCodigoBarras,
                    IdPunto = intIdPunto,
                    IdUsuario = intIdUsuario,
                    ValorAcumulado = lngValorAcumulado,
                    ValorAcumuladoConvenio = lngValorAcumuladoConvenio,
                    CodigoDeBarrasDescargar = strCodigoDeBarrasDescargar
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                strError = string.Concat("Error en ValidarCodigoBarras_RepositorioCodigodeBarras: ", ex.Message);
            }

            return objConsultaCodigodeBarras;

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

            ConsultaCodigodeBarras objConsultaCodigodeBarras = null;
            strError = string.Empty;

            try
            {

                objConsultaCodigodeBarras = _cnn.Query<ConsultaCodigodeBarras>("SP_TransferirSaldoCodigoBarras", param: new
                {
                    CodigoACargar = strCodigoACargar,
                    CodigoDeBarrasDescargar = strCodigoDeBarrasDescargar,
                    ValorAcumulado = lngValorAcumulado,
                    IdUsuario = intIdUsuario                    
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                strError = string.Concat("Error en ValidarCodigoBarras_RepositorioCodigodeBarras: ", ex.Message);
            }

            return objConsultaCodigodeBarras;

        }

        #endregion

    }
}
