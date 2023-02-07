using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{

	public class RepositorioDetalleRecoleccionDocumento : IRepositorioDetalleRecoleccionDocumento
	{

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioDetalleRecoleccionDocumento()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Inserta una recoleccion de documentos.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioCreacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionDocumento> modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionDocumento", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioCreacion = intIdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoRecoleccion.ToString(),
                                                            col2 = x.IdMedioPagoFactura.ToString(),
                                                            col3 = x.RevisionTaquillero.ToString(),
                                                            col4 = x.NumeroSobre
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionDocumento", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }



        /// <summary>
        /// RDSH: Inserta una recoleccion de documentos.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioCreacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool InsertarGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable<DetalleRecoleccionDocumento> modelo, int intIdUsuarioSupervisor, 
                                    out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionDocumentoGeneral", param: new
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
                                                            col2 = x.IdMedioPagoFactura.ToString(),
                                                            col3 = x.RevisionTaquillero.ToString(),
                                                            col4 = x.NumeroSobre
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionDocumento", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }
        /// <summary>
        /// RDSH: Retorna el detalle de una recoleccion de documentos por id recoleccion.
        /// </summary>
        /// <param name="IdRecoleccion"></param>
        /// <returns></returns>
        public IEnumerable<DetalleRecoleccionDocumento> ObtenerPorIDRecoleccion(int IdRecoleccion)
        {
            try
            {
                var objDetalleRecoleccionMonetaria = _cnn.Query<DetalleRecoleccionDocumento>("SP_ObtenerDetalleRecoleccionDocumento", param: new
                {
                    IdRecoleccion = IdRecoleccion
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return objDetalleRecoleccionMonetaria;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ObtenerPorIDRecoleccion_RepositorioDetalleRecoleccionDocumento", ex.Message));
            }
        }

        /// <summary>
        /// RDSH: Actualiza una recoleccion de documentos.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioModificacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionDocumento> modelo, out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_ActualizarDetalleRecoleccionDocumento", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioModificacion = intIdUsuarioModificacion,
                    FechaModificacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdDetalleRecoleccionDocumento.ToString(),
                                                            col2 = x.RevisionSupervisor.ToString(),
                                                            col3 = x.RevisionNido.ToString()
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioDetalleRecoleccionDocumento", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        #endregion

    }
}
