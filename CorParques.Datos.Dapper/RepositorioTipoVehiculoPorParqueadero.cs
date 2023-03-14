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

    public class RepositorioTipoVehiculoPorParqueadero : IRepositorioTipoVehiculoPorParqueadero
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioTipoVehiculoPorParqueadero()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza la disponibilidad del parqueadero.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(TipoVehiculoPorParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("SP_ActualizarTB_TipoVehiculoPorParqueadero", param: new
            {
                IdTipoVehiculoPorParqueadero = modelo.IdTipoVehiculoPorParqueadero,
                IdTipoVehiculo = modelo.IdTipoVehiculo,
                Cantidad = modelo.Cantidad,
                EspaciosPorVehiculo = modelo.EspaciosPorVehiculo,
                EspaciosReservados = modelo.EspaciosReservados,
                IdEstado = modelo.IdEstado,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: borrado logico disponibilidad parqueadero.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Eliminar(TipoVehiculoPorParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("SP_EliminarTipoVehiculoPorParqueadero", param: new
            {
                IdTipoVehiculoPorParqueadero = modelo.IdTipoVehiculoPorParqueadero,               
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        public bool Insertar(TipoVehiculoPorParqueadero modelo, out string error)
        {
            error = _cnn.Query<string>("SP_InsertarTipoVehiculoPorParqueadero", param: new
            {                
                IdTipoVehiculo = modelo.IdTipoVehiculo,
                Cantidad = modelo.Cantidad,
                EspaciosPorVehiculo = modelo.EspaciosPorVehiculo,
                EspaciosReservados = modelo.EspaciosReservados,
                IdEstado = modelo.IdEstado,               
                IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                        
            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// RDSH: Obtiene la lista de los tipos de vehiculo por parqueadero activos para la grilla principal.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoVehiculoPorParqueadero> ObtenerLista()
        {
            var lista = _cnn.Query<TipoVehiculoPorParqueadero>("SP_ObtenerTipoVehiculoPorParqueadero", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        /// <summary>
        /// RDSH: Retorna un objeto TipoVehiculoPorParqueadero para la edicion.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TipoVehiculoPorParqueadero ObtenerPorId(int Id)
        {
            var objTipoVehiculoPorParqueadero = _cnn.GetList<TipoVehiculoPorParqueadero>().Where(x => x.IdTipoVehiculoPorParqueadero == Id).FirstOrDefault();
            return objTipoVehiculoPorParqueadero;
        }

        /// <summary>
        /// RDSH: Obtiene la lista de los tipos de vehiculo por parqueadero activos y filtrados por id tipo de vehiculo.
        /// </summary>
        /// <param name="IdTipoVehiculo"></param>
        /// <returns></returns>
        public IEnumerable<TipoVehiculoPorParqueadero> ObtenerPorIdTipoVehiculo(int IdTipoVehiculo)
        {
            var lista = _cnn.GetList<TipoVehiculoPorParqueadero>().Where(x => x.IdTipoVehiculo == IdTipoVehiculo && x.IdEstado == 1).ToList();
            return lista;
        }



        /// <summary>
        /// RDSH: Valida si existe un id tipo vehiculo en estado activo, esto para que no permita tener dos tipos vehiculos iguales activos.
        /// </summary>
        /// <returns></returns>
        //private string ValidarSiExiste(int IdTipoVehiculo)
        //{
        //    string strRetorno = string.Empty;

        //    try
        //    {
        //        var objTipoVehiculoPorParqueadero = _cnn.GetList<TipoVehiculoPorParqueadero>().Where(x => x.IdTipoVehiculo == IdTipoVehiculo && x.IdEstado == 1).FirstOrDefault();
        //        if (objTipoVehiculoPorParqueadero != null)
        //        {
        //            strRetorno = "Existe";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        strRetorno = string.Concat("Ocurrio un error en ValidarSiExiste_RepositorioTipoVehiculoPorParqueadero", ex.Message);
        //    }

        //    return strRetorno;
        //}

        #endregion
    }
}
