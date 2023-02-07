using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using CorParques.Transversales.Util;
using System.Linq;

namespace CorParques.Datos.Dapper
{

	public class RepositorioPlaneacion : RepositorioBase<Planeacion>,  IRepositorioPlaneacion
	{

        #region Metodos

        /// <summary>
        /// RDSH: Obtiene la lista de indicadores.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaIndicadores()
        {
            
            try
            {
                var objListaIndicadores = _cnn.GetList<Indicadores>().Select(x => new TipoGeneral { Id = x.IdIndicador, Nombre = x.Nombre });
                return objListaIndicadores.OrderBy(x=>x.Nombre);
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioPlaneacion_ObtenerListaIndicadores");
                return null;
            }

        }

        /// <summary>
        /// RDSH: Retorna una planeacion filtrada por id indicador y fecha.
        /// </summary>
        /// <param name="intIdIndicador"></param>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        public IEnumerable<PlaneacionAuxiliar> ConsultarPlaneacion(int intIdIndicador, string strFecha)
        {
            try
            {
                var objRetorno = _cnn.Query<PlaneacionAuxiliar>("SP_ObtenerPlaneacion", param: new
                {
                    IdIndicador = intIdIndicador,
                    Fecha = strFecha
                }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                return objRetorno;

            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioPlaneacion_ConsultarPlaneacion");
                return null;
            }

        }

        /// <summary>
        /// RDSH: Inserta la planeacion mensual.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Planeacion modelo, out string error)
        {

            try
            {
                error = _cnn.Query<string>("SP_InsertarPlaneacion", param: new
                {
                    IdIndicador = modelo.IdIndicador,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion,
                    Planeacion = Utilidades.convertTable(modelo.Planeaciones
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdPlaneacion.ToString(),
                                                            col2 = x.FechaTexto.ToString(),
                                                            col3 = x.Valor.ToString().Replace(",", ".")

                                                        })),
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en RepositorioPlaneacion_Insertar: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);

        }
        #endregion

    }
}
