using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioDetalleRecoleccionNovedad : RepositorioBase<DetalleRecoleccionNovedad>,  IRepositorioDetalleRecoleccionNovedad
	{

        #region Metodos

        /// <summary>
        /// RDSH: Inserta una recoleccion de novedad arqueo.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioCreacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionNovedad> modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionNovedad", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioCreacion = intIdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoRecoleccion.ToString(),
                                                            col2 = x.IdNovedadArqueo.ToString(),
                                                            col3 = x.RevisionTaquillero.ToString(),
                                                            col4 = x.NumeroSobre
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionNovedad", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }


        public bool InsertarGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionNovedad> modelo, int intIdUsuarioSupervisor, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionNovedadGeneral", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioCreacion = intIdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    IdUsuarioSupervisor = intIdUsuarioSupervisor,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoRecoleccion.ToString(),
                                                            col2 = x.IdNovedadArqueo.ToString(),
                                                            col3 = x.RevisionTaquillero.ToString(),
                                                            col4 = x.NumeroSobre
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionNovedad", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }


        /// <summary>
        /// RDSH: Retorna el detalle de una recoleccion de novedad por id recoleccion.
        /// </summary>
        /// <param name="IdRecoleccion"></param>
        /// <returns></returns>
        public IEnumerable<DetalleRecoleccionNovedad> ObtenerPorIDRecoleccion(int IdRecoleccion)
        {
            try
            {
                var objDetalleRecoleccionNovedad = _cnn.Query<DetalleRecoleccionNovedad>("SP_ObtenerDetalleRecoleccionNovedad", param: new
                {
                    IdRecoleccion = IdRecoleccion
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return objDetalleRecoleccionNovedad;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ObtenerPorIDRecoleccion_RepositorioDetalleRecoleccionNovedad", ex.Message));
            }
        }

        /// <summary>
        /// RDSH: Actualiza una recoleccion de novedad.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioModificacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionNovedad> modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_ActualizarDetalleRecoleccionNovedad", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioModificacion = intIdUsuarioModificacion,
                    FechaModificacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdDetalleRecoleccionNovedad.ToString(),
                                                            col2 = x.RevisionSupervisor.ToString(),
                                                            col3 = x.RevisionNido.ToString()
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioDetalleRecoleccionNovedad", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        #endregion

    }
}
