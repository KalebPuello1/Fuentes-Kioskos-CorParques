using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

    public class RepositorioAdicionPedidos : RepositorioBase<AdicionPedidos>, IRepositorioAdicionPedidos
    {

        #region Metodos

        /// <summary>
        /// Retorna el detalle de un pedido valido para adiciones.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdicionPedidos> DetallePedido(string strCodigoSapPedido)
        {
            List<AdicionPedidos> objAdicionPedidos = null;

            try
            {
                objAdicionPedidos = _cnn.Query<AdicionPedidos>("SP_ObtenerInformacionParaAdicionPedido", param: new
                {
                    CodigoSapPedido = strCodigoSapPedido
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioAdicionPedidos_DetallePedido");
            }

            return objAdicionPedidos;

        }

        /// <summary>
        /// RDSH: Recibe un modelo para guardar y retorna un modelo con los consecutivos que debe imprimir.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public IEnumerable<AdicionPedidos> Guardar(IEnumerable<AdicionPedidos> modelo)
        {
            List<AdicionPedidos> objAdicionPedidos = null;
            int intIdUsuario;
            string strCodigoSapPedido = string.Empty;
            int IdPunto;
            //Debe agregarse la validacion de impresion en linea
            try
            {
                intIdUsuario = modelo.Select(x => x.IdUsuario).FirstOrDefault();
                strCodigoSapPedido = modelo.Select(x => x.CodigoSapPedido).FirstOrDefault();
                IdPunto = modelo.First().IdPunto;
                objAdicionPedidos = _cnn.Query<AdicionPedidos>("SP_GenerarAdicionPedidos", param: new
                {
                    CodigoPedido = strCodigoSapPedido,
                    IdUsuario = intIdUsuario,
                    IdPunto = IdPunto,
                    DetallePedido = Utilidades.convertTable(modelo
                                                       .Select(x => new TablaGeneral
                                                       {
                                                           col1 = x.IdProducto.ToString(),
                                                           col2 = x.Cantidad.ToString(),
                                                           col3 = x.Posicion.ToString(),
                                                           col4 = x.Valor.ToString(),
                                                           col5 = x.ConsecutivoInicial,
                                                           col6 = x.ConsecutivoFinal,
                                                           col7 = x.CodigoSapProducto
                                                       })),
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioAdicionPedidos_Guardar");
            }

            return objAdicionPedidos;

        }

        /// <summary>
        /// RDSH: Valida que un rango de brazaletes pueda ser usado en adicion de pedido.
        /// </summary>
        /// <param name="strConsecutivoInicial"></param>
        /// <param name="strConsecutivoFinal"></param>
        /// <param name="intCantidad"></param>
        /// <returns></returns>
        public string ValidarRangoConsecutivos(string strConsecutivoInicial, string strConsecutivoFinal, int intCantidad, int intIdProducto,int idUsuario)
        {
            string strResultado = string.Empty; 

            try
            {
                strResultado = _cnn.Query<string>("SP_ValidarConsecutivosAdicionPedidos", param: new
                {
                    IdProducto = intIdProducto,
                    ConsecutivoInicial = strConsecutivoInicial,
                    ConsecutivoFinal = strConsecutivoFinal,
                    Cantidad = intCantidad,
                    IdUsuario = idUsuario
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {                
                Utilidades.RegistrarError(ex, "RepositorioAdicionPedidos_ValidarRangoConsecutivos");
                strResultado = "Error en la validación de rangos consecutivos.";
            }

            return strResultado;

        }

        #endregion

    }
}
