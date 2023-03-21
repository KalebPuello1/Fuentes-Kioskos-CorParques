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
    public class RepositorioTarifasParqueadero : IRepositorioTarifasParqueadero
    {


        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioTarifasParqueadero()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza la informacion de una tarifa.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(TarifasParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("ActualizarTarifaParqueadero", param: new
            {
                IdTarifaParqueadero = modelo.Id,
                IdTipoTarifaParqueadero = modelo.IdTipoTarifaParqueadero,
                IdTipoVehiculo = modelo.IdTipoVehiculo,
                IdEstado = modelo.IdEstado,
                Nombre = modelo.Nombre,
                //Cantidad = modelo.Cantidad, 
                Valor = modelo.Valor,
                IdUsuarioModificacion = modelo.UsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        public bool Eliminar(TarifasParqueadero modelo, out string error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Inserta la informacion de una tarifa.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(TarifasParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("InsertarTarifaParqueadero", param: new
            {
                IdTipoTarifaParqueadero = modelo.IdTipoTarifaParqueadero,
                IdTipoVehiculo = modelo.IdTipoVehiculo,
                IdEstado = modelo.IdEstado,
                Nombre = modelo.Nombre,
                //Cantidad = modelo.Cantidad,
                Valor = modelo.Valor,
                IdUsuarioCreacion = modelo.UsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Obtiene una tarifa por id.
        /// </summary>
        /// <param name="intId"></param>
        /// <returns></returns>
        public TarifasParqueadero Obtener(int intId)
        {
            try
            {
                return _cnn.Get<TarifasParqueadero>(intId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// RDSH: Obtiene el listado de tarifas para la pantalla inicial.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TarifasParqueadero> ObtenerLista()
        {
            var lista = _cnn.Query<TarifasParqueadero>("ObtenerTarifasParqueadero", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        /// <summary>
        /// RDSH: Retorna las tarifas parqueadero con estado activo.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _cnn.GetList<TarifasParqueadero>().Where(x => x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });            
        }

        /// <summary>
        /// RDSH: Retorna las tarifas asociadas a un tipo de vehiculo.
        /// </summary>
        /// <param name="IdTipoVehiculo"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTarifaPorIdTipoVehiculo(int IdTipoVehiculo)
        {            
            return _cnn.GetList<TarifasParqueadero>().Where(x => x.IdTipoVehiculo == IdTipoVehiculo && x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });                
        }

        #endregion
    }
}
