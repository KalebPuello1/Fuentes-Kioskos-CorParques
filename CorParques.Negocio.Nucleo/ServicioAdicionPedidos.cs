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

    public class ServicioAdicionPedidos : IServicioAdicionPedidos
    {

        private readonly IRepositorioAdicionPedidos _repositorio;

        #region Constructor

        public ServicioAdicionPedidos(IRepositorioAdicionPedidos repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Retorna el detalle de un pedido valido para adiciones.
        /// </summary>
        /// <param name="strCodigoSapPedido"></param>
        /// <returns></returns>
        public IEnumerable<AdicionPedidos> DetallePedido(string strCodigoSapPedido)
        {
            return _repositorio.DetallePedido(strCodigoSapPedido);
        }

        #endregion

        #region No Implementados

        public bool Actualizar(AdicionPedidos modelo)
        {
            throw new NotImplementedException();
        }

        public AdicionPedidos Crear(AdicionPedidos modelo)
        {
            throw new NotImplementedException();
        }

        public AdicionPedidos Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdicionPedidos> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Valida que un rango de brazaletes pueda ser usado en adicion de pedido.
        /// </summary>
        /// <param name="strConsecutivoInicial"></param>
        /// <param name="strConsecutivoFinal"></param>
        /// <param name="intCantidad"></param>
        /// <returns></returns>
        public string ValidarRangoConsecutivos(string strConsecutivoInicial, string strConsecutivoFinal, int intCantidad, int intIdProducto, int idUsuario)
        {
            return _repositorio.ValidarRangoConsecutivos(strConsecutivoInicial, strConsecutivoFinal, intCantidad, intIdProducto, idUsuario);
        }

        /// <summary>
        /// RDSH: Recibe un modelo para guardar y retorna un modelo con los consecutivos que debe imprimir.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public IEnumerable<AdicionPedidos> Guardar(IEnumerable<AdicionPedidos> modelo)
        {
            return _repositorio.Guardar(modelo);
        }

        #endregion

    }
}
