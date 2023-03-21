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

    public class RepositorioDetalleRecoleccionBrazalete : RepositorioBase<DetalleRecoleccionBrazalete>, IRepositorioDetalleRecoleccionBrazalete
    {

        #region Metodos
        
        /// <summary>
        /// RDSH: Inserta el detalle de una recoleccion de brazaletes (boleteria).
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioCreacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionBrazalete> modelo, out string error)
        {

            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionBrazalete", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioCreacion = intIdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoBrazalete.ToString(),
                                                            col2 = x.CantidadTaquillero.ToString(),
                                                            col3 = x.IdAperturaBrazaleteDetalle.ToString()
                                                            
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionBrazalete", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public bool InsertarGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionBrazalete> modelo, int intIdUsuarioSupervisor, out string error)
        {

            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionBrazaleteGeneral", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioCreacion = intIdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    IdUsuarioSupervisor = intIdUsuarioSupervisor,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoBrazalete.ToString(),
                                                            col2 = x.CantidadTaquillero.ToString(),
                                                            col3 = x.IdAperturaBrazaleteDetalle.ToString()

                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionBrazalete", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public IEnumerable<DetalleRecoleccionBrazalete> ObtenerPorIDRecoleccion(int IdRecoleccion)
        {
            try
            {
                var objDetalleRecoleccionBrazalete = _cnn.Query<DetalleRecoleccionBrazalete>("SP_ObtenerDetalleRecoleccionBrazalete", param: new
                {
                    IdRecoleccion = IdRecoleccion
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return objDetalleRecoleccionBrazalete;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ObtenerPorIDRecoleccion_RepositorioDetalleRecoleccionBrazalete", ex.Message));
            }
        }

        /// <summary>
        /// RDSH: Actualiza el detalle de una recoleccion de brazaletes (boleteria).
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioModificacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionBrazalete> modelo, out string error)
        {

            try
            {
                error = _cnn.Query<string>("SP_ActualizarDetalleRecoleccionBrazalete", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioModificacion = intIdUsuarioModificacion,
                    FechaModificacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdDetalleRecoleccionBrazalete.ToString(),
                                                            col2 = x.CantidadSupervisor.ToString(),
                                                            col3 = x.CantidadNido.ToString(),
                                                            col4 = x.IdAperturaBrazaleteDetalle.ToString()

                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioDetalleRecoleccionBrazalete", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        #endregion

    }
}
