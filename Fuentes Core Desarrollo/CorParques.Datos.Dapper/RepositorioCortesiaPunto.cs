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

namespace CorParques.Datos.Dapper
{

    public class RepositorioCortesiaPunto : IRepositorioCortesiaPunto
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioCortesiaPunto()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza la relacion de las atracciones a las que se les puede imprimir boletos de cortesia.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(CortesiaPunto modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_ActualizarCortesiaPunto", param: new
                {
                    IdCortesiaPunto = modelo.IdCortesiaPunto,
                    IdPuntoDestreza = modelo.IdPuntoDestreza,
                    IdProducto = modelo.IdProducto,
                    Cantidad = modelo.Cantidad,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Actualizar_RepositorioCortesiaPunto: ", ex.Message);
            }
                
            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// Borrado logico de la configuracion de una cortesia.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Eliminar(CortesiaPunto modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_EliminarCortesiaPunto", param: new
                {
                    IdCortesiaPunto = modelo.IdCortesiaPunto,                                
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Eliminar_RepositorioCortesiaPunto: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Inserta la relacion de las atracciones a las que se les puede imprimir boletos de cortesia.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(CortesiaPunto modelo, out string error)
        {         
            error = string.Empty;

            try
            {

                error = _cnn.Query<string>("SP_InsertarCortesiaPunto", param: new
                {
                    IdPuntoDestreza = modelo.IdPuntoDestreza,
                    IdProducto = modelo.IdProducto,
                    Cantidad = modelo.Cantidad,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                

            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Insertar_RepositorioCortesiaPunto: ", ex.Message);               
            }

            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// RDSH: Retorna una lista de CortesiaPunto donde se puede filtrar por IdDestreza o por IdAtraccion o para traer todas enviar cero en ambos parametros.
        /// </summary>
        /// <param name="IdDestreza"></param>
        /// <param name="IdAtraccion"></param>
        /// <returns></returns>        
        public IEnumerable<CortesiaPunto> ObtenerPorDestrezaAtraccion(int IdDestreza, int IdAtraccion)
        {
            var lista = _cnn.Query<CortesiaPunto>("SP_ObtenerCortesiaPunto", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;            
        }

        /// <summary>
        /// RDSH: Retorna la configuracion de una cortesia para su edicion.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CortesiaPunto ObtenerPorId(int Id)
        {
            var objCortesiaPunto = _cnn.GetList<CortesiaPunto>().Where(x => x.IdCortesiaPunto == Id).FirstOrDefault();
            return objCortesiaPunto;
        }
        
        /// <summary>
        /// OEGA: Retorna los Productos con TipoProducto atracciones y destrezas.
        /// </summary>
        /// <param name="CodTipoProducto"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerProductos(string CodTipoProducto)
        {
            var objProductos = _cnn.Query<TipoGeneral>("SP_GetProductos", param: new
            {
                CodTipoProducto = CodTipoProducto

            }, commandType: System.Data.CommandType.StoredProcedure).ToList();

            return objProductos;
        }

        #endregion
    }
}
