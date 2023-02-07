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
    public class RepositorioTipoTarifaParqueadero : IRepositorioTipoTarifaParqueadero
    {


        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioTipoTarifaParqueadero()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Retorna todas los tipos de tarifas de parqueadero.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoTarifaParqueadero> ObtenerLista()
        {
            var lista = _cnn.Query<TipoTarifaParqueadero>("ObtenerTiposTarifasParqueadero", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        /// <summary>
        /// RDSH: Retorna una lista simple de tipos de tarifas.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {            
            return _cnn.GetList<TipoTarifaParqueadero>().Where(x => x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
        }

        public bool Actualizar(TipoTarifaParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("ActualizarTipoTarifaParqueadero", param: new
            {
                IdTipoTarifaParqueadero = modelo.Id,
                Nombre = modelo.Nombre,
                Descripcion = modelo.Descripcion,
                IdEstado = modelo.IdEstado,
                IdUsuarioModificacion = modelo.UsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        public bool Eliminar(TipoTarifaParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("EliminarTipoTarifaParqueadero", param: new
            {
                IdCargueBrazalete = modelo.Id
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        public bool Insertar(TipoTarifaParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("InsertarTipoTarifaParqueadero", param: new
            {
                Nombre = modelo.Nombre,
                Descripcion = modelo.Descripcion,
                IdEstado = modelo.IdEstado,
                IdUsuarioCreacion = modelo.UsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Retorna un tipo de tarifa por id.
        /// </summary>
        /// <param name="idTipoBrazalete"></param>
        /// <returns></returns>
        public TipoTarifaParqueadero Obtener(int intId)
        {
            try
            {
                return _cnn.Get<TipoTarifaParqueadero>(intId);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        #endregion
    }
}
