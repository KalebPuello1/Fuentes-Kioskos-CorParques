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

    public class RepositorioDetalleRecoleccionMonetaria : IRepositorioDetalleRecoleccionMonetaria
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioDetalleRecoleccionMonetaria()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Inserta una recoleccion monetaria.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioCreacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, IEnumerable <DetalleRecoleccionMonetaria> modelo, out string error)
        {
            
            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionMonetaria", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioCreacion = intIdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoRecoleccion.ToString(),
                                                            col2 = x.IdTipoDenominacion.ToString(),
                                                            col3 = x.CantidadTaquillero.ToString(),
                                                            col4 = x.NumeroSobre
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionMonetaria", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        
        public bool InsertarRecoleccionGeneral(int intIdRecoleccion, int intIdEstado, int intIdUsuarioCreacion, float ValorRecoleccionBase, float ValorRecoleccionCorte, int idSupervisor,
                                               out string error)
        {
            try
            {
                error = _cnn.Query<string>("SP_InsertarDetalleRecoleccionMonetariaGeneral", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioCreacion = intIdUsuarioCreacion,
                    FechaCreacion = DateTime.Now,
                    ValorCorte = ValorRecoleccionCorte,
                    ValorBase = ValorRecoleccionBase,
                    IdUsuarioSupervisor = idSupervisor                    
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioDetalleRecoleccionMonetaria", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }


        /// <summary>
        /// Retorna el detalle de una recoleccion monetaria por el id de la recoleccion.
        /// </summary>
        /// <param name="IdRecoleccion"></param>
        /// <returns></returns>
        public IEnumerable<DetalleRecoleccionMonetaria> ObtenerPorIDRecoleccion(int IdRecoleccion)
        {

            try
            {
                var objDetalleRecoleccionMonetaria = _cnn.Query<DetalleRecoleccionMonetaria>("SP_ObtenerDetalleRecoleccionMonetaria", param: new
                {
                    IdRecoleccion = IdRecoleccion
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return objDetalleRecoleccionMonetaria;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ObtenerPorIDRecoleccion_RepositorioDetalleRecoleccionMonetaria", ex.Message));
            }
        }


        /// <summary>
        /// RDSH: Actualiza una recoleccion monetaria.
        /// </summary>
        /// <param name="intIdRecoleccion"></param>
        /// <param name="intIdEstado"></param>
        /// <param name="intIdUsuarioCreacion"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(int intIdRecoleccion, int intIdEstado, int intIdUsuarioModificacion, IEnumerable<DetalleRecoleccionMonetaria> modelo, out string error)
        {

            try
            {
                error = _cnn.Query<string>("SP_ActualizarDetalleRecoleccionMonetaria", param: new
                {
                    IdRecoleccion = intIdRecoleccion,
                    IdEstado = intIdEstado,
                    IdUsuarioModificacion = intIdUsuarioModificacion,
                    FechaModificacion = DateTime.Now,
                    DetalleRecoleccion = Utilidades.convertTable(modelo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdDetalleRecoleccionMonetaria.ToString(),
                                                            col2 = x.CantidadSupervisor.ToString(),
                                                            col3 = x.CantidadNido.ToString()
                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioDetalleRecoleccionMonetaria", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        #endregion

    }
}
