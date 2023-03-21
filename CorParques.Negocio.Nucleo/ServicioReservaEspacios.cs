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

    public class ServicioReservaEspacios : IServicioReservaEspacios
    {

        private readonly IRepositorioReservaEspacios _repositorio;

        #region Constructor

        public ServicioReservaEspacios(IRepositorioReservaEspacios repositorio)
        {

            _repositorio = repositorio;
        }

        #endregion

        #region Metodos

        public ReservaEspacios Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<TipoGeneral> ObtenerEspaciosxTipo(int IdTipoEsapcio)
        {
            return _repositorio.ObtenerEspaciosxTipo(IdTipoEsapcio);
        }

        public IEnumerable<TipoGeneral> ObtenerTipoEspacios()
        {
            return _repositorio.ObtenerTipoEspacios();
        }

        public IEnumerable<TipoGeneral> ObtenerEspacios()
        {
            return _repositorio.ObtenerEspacios();
        }

        /// <summary>
        /// RDSH: Retorna las reservas que se han realizado para el espacio y la fecha seleccionada.
        /// </summary>
        /// <param name="intIdEspacio"></param>
        /// <param name="strFechaReserva"></param>
        /// <returns></returns>
        public IEnumerable<ReservaEspacios> ObtenerReservaEspacios(int intIdReserva, string strFechaReserva)
        {
            return _repositorio.ObtenerReservaEspacios(intIdReserva, strFechaReserva);
        }

        /// <summary>
        /// RDSH: Actualiza una reserva de espacios.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(ReservaEspacios modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        /// <summary>
        /// RDSH: Inserta una reserva de espacios.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(ReservaEspacios modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        /// <summary>
        /// RDSH: Retorna los tipos de reserva activos ordenados por nombre. 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoReserva> ObtenerTiposReserva()
        {
            return _repositorio.ObtenerTiposReserva();
        }

        /// <summary>
        /// RDSH: Retorna el detalle de un pedido sap para mostrarlo en la reserva de espacios.
        /// </summary>
        /// <param name="strCodigoSapPedido"></param>
        /// <returns></returns>
        public IEnumerable<ReservaEspaciosAuxiliar> ObtenerDetallePedido(string strCodigoSapPedido, string strFechaReserva)
        {
            return _repositorio.ObtenerDetallePedido(strCodigoSapPedido, strFechaReserva);
        }

        /// <summary>
        /// RDSH: Elimina una reserva.
        /// </summary>
        /// <param name="intIdReservaEspacio"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Eliminar(int intIdReservaEspacio, int intIdUsuario, out string error)
        {
            return _repositorio.Eliminar(intIdReservaEspacio, intIdUsuario, out error);
        }

        #endregion

        #region Metodos no implementados


        public bool Actualizar(ReservaEspacios modelo)
        {
            throw new NotImplementedException();
        }

        public ReservaEspacios Crear(ReservaEspacios modelo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReservaEspacios> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
