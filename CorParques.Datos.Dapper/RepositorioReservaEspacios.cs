using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioReservaEspacios : RepositorioBase<ReservaEspacios>,  IRepositorioReservaEspacios
	{

        public IEnumerable<TipoGeneral> ObtenerEspaciosxTipo(int IdTipoEsapcio)
        {

            var respuesta = _cnn.Query<Espacio>("SP_ObtenerEspaciosxTipo", param: new { IdTipoEsapcio = IdTipoEsapcio }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return respuesta.Select(x => new TipoGeneral { Id = x.IdEspacio, Nombre = x.Nombre });
        }

        public IEnumerable<TipoGeneral> ObtenerTipoEspacios()
        {
            return _cnn.Query<TipoEspacio>("SP_ObtenerTipoEspacios", commandType: System.Data.CommandType.StoredProcedure).ToList().Select(x => new TipoGeneral { Id = x.IdTipoEspacio, Nombre = x.Nombre });
        }

        public IEnumerable<TipoGeneral> ObtenerEspacios()
        {
            return _cnn.Query<TipoGeneral>("SP_ObtenerEspacios",
                commandType: System.Data.CommandType.StoredProcedure);
        }

        /// <summary>
        /// RDSH: Retorna las reservas que se han realizado para el espacio y la fecha seleccionada.
        /// </summary>
        /// <param name="intIdEspacio"></param>
        /// <param name="strFechaReserva"></param>
        /// <returns></returns>
        public IEnumerable<ReservaEspacios> ObtenerReservaEspacios(int intIdReserva, string strFechaReserva)
        {
            return _cnn.Query<ReservaEspacios>("SP_ObtenerReservaEspacios", param: new  {
                IdReserva = intIdReserva,
                FechaReserva = strFechaReserva
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// RDSH: Actualiza una reserva de espacios.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(ReservaEspacios modelo, out string error)
        {
            
            try
            {
                error = _cnn.Query<string>("SP_ActualizarReservaEspacios", param: new
                {
                    IdReservaEspacios = modelo.IdReservaEspacios,
                    IdEspacio = modelo.IdEspacio,                    
                    IdTipoReserva = modelo.IdTipoReserva,
                    CodigoSapPedido = modelo.CodigoSapPedido,
                    NombrePersona = modelo.NombrePersona,
                    FechaReserva = modelo.FechaReserva,
                    HoraInicio = modelo.HoraInicio,
                    HoraFin = modelo.HoraFin,
                    Observaciones = modelo.Observaciones.Trim(),
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion                    
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioReservaEspacios: ", ex.Message);
            }

            if (error.Trim().IndexOf("[S]") >= 0)
            {                
                return true;
            }
            else
            {
                return string.IsNullOrEmpty(error);
            }            
        }

        /// <summary>
        /// RDSH: Inserta una reserva de espacios.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(ReservaEspacios modelo, out string error)
        {

            try
            {
                error = _cnn.Query<string>("SP_InsertarReservaEspacios", param: new
                {
                    IdEspacio = modelo.IdEspacio,
                    IdTipoEspacio = modelo.IdTipoEspacio,
                    IdTipoReserva = modelo.IdTipoReserva,
                    CodigoSapPedido = modelo.CodigoSapPedido,
                    NombrePersona = modelo.NombrePersona,
                    FechaReserva = modelo.FechaReserva,
                    HoraInicio = modelo.HoraInicio,
                    HoraFin = modelo.HoraFin,
                    Observaciones = modelo.Observaciones.Trim(),
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion                    
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioReservaEspacios: ", ex.Message);
            }

            if (error.Trim().IndexOf("[S]") >= 0)
            {
                error = error.Replace("[S]", "");
                return true;
            }
            else
            {
                return string.IsNullOrEmpty(error);
            }

            

        }


        /// <summary>
        /// RDSH: Elimina una reserva.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Eliminar(int intIdReservaEspacio, int intIdUsuario, out string error)
        {

            try
            {
                error = _cnn.Query<string>("SP_EliminarReservaEspacios", param: new
                {
                    IdReservaEspacios = intIdReservaEspacio,
                    IdUsuarioModificacion = intIdUsuario,
                    FechaModificacion = DateTime.Now
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Eliminar_RepositorioReservaEspacios: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// RDSH: Retorna los tipos de reserva activos ordenados por nombre.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoReserva> ObtenerTiposReserva()
        {
            return _cnn.Query<TipoReserva>("SP_ObtenerTiposReserva", commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// RDSH: Retorna el detalle de un pedido sap para mostrarlo en la reserva de espacios.
        /// </summary>
        /// <param name="strCodigoSapPedido"></param>
        /// <returns></returns>
        public IEnumerable<ReservaEspaciosAuxiliar> ObtenerDetallePedido(string strCodigoSapPedido, string strFechaReserva)
        {
            return _cnn.Query<ReservaEspaciosAuxiliar>("SP_ObtenerDetallePedidoReservaEspacios", param: new  {
                CodigoSapPedido = strCodigoSapPedido,
                FechaReserva = strFechaReserva
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

    }
}
