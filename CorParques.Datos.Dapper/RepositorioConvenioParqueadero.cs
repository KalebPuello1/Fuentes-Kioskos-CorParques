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

    public class RepositorioConvenioParqueadero : IRepositorioConvenioParqueadero
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioConvenioParqueadero()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        /// <summary>
        /// RDSH: Actualiza un convenio.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(ConvenioParqueadero modelo, out string error)
        {

            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_ActualizarConvenioParqueadero", param: new
                {
                    IdConvenioParqueadero = modelo.Id,                    
                    Documento = modelo.Documento,
                    Nombre = modelo.Nombre,
                    Apellido = modelo.Apellido,
                    Area = modelo.Area,
                    IdTipoConvenioParqueadero = modelo.IdTipoConvenioParqueadero,                    
                    IdEstado = modelo.IdEstado,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion,
                    FechaVigencia = modelo.FechaVigencia,
                    Vehiculos = Utilidades.convertTable(modelo.objAutorizacionVehiculo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoVehiculo.ToString(),
                                                            col2 = x.Placa.ToString()
                                                        }))
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioConvenioParqueadero: ", ex.Message);
            }
         
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Borrado logico de convenio.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Eliminar(ConvenioParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("SP_EliminarConvenioParqueadero", param: new
            {
                IdConvenioParqueadero = modelo.Id,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Inserta un convenio.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(ConvenioParqueadero modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarConvenioParqueadero", param: new
                {                    
                    Documento = modelo.Documento,
                    Nombre = modelo.Nombre,
                    Apellido = modelo.Apellido,
                    Area = modelo.Area,
                    IdTipoConvenioParqueadero = modelo.IdTipoConvenioParqueadero,                    
                    IdEstado = modelo.IdEstado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion,
                    FechaVigencia = modelo.FechaVigencia,
                    Vehiculos = Utilidades.convertTable(modelo.objAutorizacionVehiculo
                                                        .Select(x => new TablaGeneral
                                                        {
                                                            col1 = x.IdTipoVehiculo.ToString(),
                                                            col2 = x.Placa.ToString()
                                                        }))
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioConvenioParqueadero: ", ex.Message);
            }            

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Retorna un convenio por id para su edicion.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ConvenioParqueadero ObtenerPorId(int Id)
        {

            ConvenioParqueadero objConvenioParqueadero = null;

            try
            {
                objConvenioParqueadero = _cnn.Query<ConvenioParqueadero>("SP_ObtenerConvenioParqueaderoPorId", param: new
                {
                    IdConvenioParqueadero = Id
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Concat("Error en ObtenerPorId_RepositorioConvenioParqueadero: ", ex.Message));
            }

            return objConvenioParqueadero;

        }

        /// <summary>
        /// RDSH: Retorna un convenio por placa.
        /// </summary>
        /// <param name="Placa"></param>
        /// <returns></returns>
        public ConvenioParqueadero ObtenerPorPlaca(string Placa)
        {
            var ConvenioParqueadero = _cnn.Query<ConvenioParqueadero>("SP_ObtenerConvenioParqueaderoPorPlaca", param: new
            {
                Placa = Placa
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return ConvenioParqueadero;
        }

        /// <summary>
        /// RDSH: Retorna los convenios creados a la grilla principal.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ConvenioParqueadero> ObtenerLista()
        {
            var lista = _cnn.Query<ConvenioParqueadero>("SP_ObtenerConvenioParqueadero", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        public IEnumerable<EstructuraEmpleado> ObtenerListaEmpleados()
        {
            var lista = _cnn.Query<EstructuraEmpleado>("SP_ObtenerEstructuraEmpleado", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

    }
}
