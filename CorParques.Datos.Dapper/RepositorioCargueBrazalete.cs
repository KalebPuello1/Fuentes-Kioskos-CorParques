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
    public class RepositorioCargueBrazalete : IRepositorioCargueBrazalete
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioCargueBrazalete()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Cambia el estado de un rango de brazaletes CARGUE_BRAZALETE, elimina los brazaletes en estado Inactivo de la tabla de detalle BRAZALETES.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(CargueBrazalete modelo, out string error)
        {
            error = _cnn.Query<string>("ActualizarCargueBrazalete", param: new
            {
                IdCargueBrazalete = modelo.IdCargueBrazalete,               
                Estado = modelo.IdEstado,                
                UsuarioModificacion = modelo.UsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion                
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Inserta el rango de brazaletes en la tabla CARGUE_BRAZALETE y en la tabla BRAZALETES el detalle (uno por uno).
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(CargueBrazalete modelo, out string error)
        {
            error = _cnn.Query<string>("InsertarCargueBrazalete", param: new
            {
                IdCargueBrazalete = modelo.IdCargueBrazalete,
                TipoBrazalete = modelo.IdTipoBrazalete,
                Descripcion = modelo.Descripcion,
                ConsecutivoInicial = modelo.ConsecutivoInicial,
                ConsecutivoFinal = modelo.ConsecutivoFinal,
                Estado = modelo.IdEstado,
                UsuarioCreacion = modelo.UsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion,
                UsuarioModificacion = modelo.UsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Retorna la lista de cargues realizados en la tabla CARGUE_BRAZALETE.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CargueBrazalete> ObtenerLista()
        {

            var lista =  _cnn.Query<CargueBrazalete>("BuscarCargueBrasalete", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            //var lista = _cnn.GetList<CargueBrazalete>();
            return lista;
        }

        /// <summary>
        /// RDSH: Retorna los tipos de brazaletes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTipoBrazalete()
        {
            //var lista = _cnn.GetList<TipoBrazalete>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });            
            var lista = _cnn.Query<TipoBrazalete>("ObtenerTiposBrazalete", null, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
            return lista;
        }

        #endregion        
    }
}
