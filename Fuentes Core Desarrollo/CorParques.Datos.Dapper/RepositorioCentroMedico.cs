using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace CorParques.Datos.Dapper
{
    /// <summary>
    /// NMSR CU006-DE-1CI-ARS-001-Configuración centro médico
    /// </summary>
    public class RepositorioCentroMedico : IRepositorioCentroMedico
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioCentroMedico()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[string.Format("{0}-{1}", ConfigurationManager.AppSettings["CnnAmbiente"] , ConfigurationManager.AppSettings["Cnn"])].ConnectionString);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(CentroMedico modelo, out string error)
        {
            error = _cnn.Query<string>("SP_InsertarCentroMedico", param: new
            {
                Ubicacion = modelo.Ubicacion,
                Descripcion = modelo.Descripcion,
                IdEstado = modelo.IdEstado,
                IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(CentroMedico modelo, out string error)
        {
            error = _cnn.Query<string>("SP_ActualizarCentroMedico", param: new
            {
                IdCentroMedico = modelo.IdCentroMedico,
                Ubicacion = modelo.Ubicacion,
                Descripcion = modelo.Descripcion,
                IdEstado = modelo.IdEstado,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// Retorna todas los Centros Médicos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CentroMedico> ObtenerListaActivos()
        {
            //var lista = _cnn.Query<CentroMedico>("ObtenerCentrosMedicos", null, commandType: System.Data.CommandType.StoredProcedure).ToList();

            try
            {

            return _cnn.GetList<CentroMedico>().Where(x => x.IdEstado == 1);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Retorna un Centro Médico por id.
        /// </summary>
        /// <param name="idCentroMedico"></param>
        /// <returns></returns>
        public CentroMedico Obtener(int idCentroMedico)
        {
            try
            {
                return _cnn.Get<CentroMedico>(idCentroMedico);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// RDSH: Retorna una lista de los centros medicos activos para cargar un combo
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaCentroMedico()
        {            
            return _cnn.GetList<CentroMedico>().Where(x => x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.IdCentroMedico, Nombre = x.Ubicacion}).OrderBy(x => x.Nombre);
        }

        /// <summary>
        /// RDSH: Retorna la lista de zona area o de ubicaciones para el reporte de centro medico.
        /// </summary>
        /// <param name="intIdCentroMedico"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaZonaAreaUbicacion(int intIdCentroMedico)
        {
            var lista = _cnn.Query<CentroMedico>("SP_ObtenerZonaAreaUbicacionPrimerosAuxilios", param: new {
                IdCentroMedico = intIdCentroMedico
            }, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre }).OrderBy(x => x.Nombre);
            return lista;
        }

        public IEnumerable<TipoGeneral> ObtenerZonas()
        {
            var lista = _cnn.Query<TipoGeneral>("SP_GetZonas", commandType: System.Data.CommandType.StoredProcedure).OrderBy(x => x.Nombre);
            return lista;
        }
    }
}
