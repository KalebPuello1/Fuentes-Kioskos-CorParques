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

    public class RepositorioCortesiaDestreza : IRepositorioCortesiaDestreza
    {

        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioCortesiaDestreza()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }
        
        #endregion

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza el estado de una cortesia destreza.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(CortesiaDestreza modelo, out string error)
        {
            error = string.Empty;

            try
            {

                error = _cnn.Query<string>("SP_ActualizarCortesiaDestreza", param: new
                {
                    IdPuntoDestreza = modelo.IdPuntoDestreza,
                    IdProducto = modelo.IdProducto,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Actualizar_RepositorioCortesiaDestreza: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        ///RDSH: Inserta una cortesia generada en destrezas.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(CortesiaDestreza modelo, out string error, out string CodigoBarras)
        {
            error = string.Empty;
            CodigoBarras = string.Empty;

            try
            {

                error = _cnn.Query<string>("SP_InsertarCortesiaDestreza", param: new
                {
                    IdPuntoDestreza = modelo.IdPuntoDestreza,
                    IdProducto = modelo.IdProducto,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                if (error.IndexOf("Error") < 0)
                {
                    CodigoBarras = error;
                    error = string.Empty;
                }

            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Insertar_RepositorioCortesiaDestreza: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);

        }

        /// <summary>
        /// RDSH: Retorna un objeto CortesiaDestreza por id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CortesiaDestreza ObtenerPorId(int Id)
        {
            return _cnn.Get<CortesiaDestreza>(Id);
        }

        /// <summary>
        /// RDSH: Retorna la cantidad de cortesias entregadas por id destreza por atracción.
        /// </summary>
        /// <param name="IdDestreza"></param>
        /// <param name="IdAtraccion"></param>
        /// <returns></returns>
        public IEnumerable<CortesiaDestreza> ObtenerPorDestrezaAtraccion(int IdDestreza, int IdAtraccion)
        {
            var lista = _cnn.Query<CortesiaDestreza>("SP_ObtnerInventarioCortesiaDestreza", param: new
            {
                IdPuntoDestreza = IdDestreza,
                IdProducto = IdAtraccion
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return lista;
        }

        /// <summary>
        /// RDSH: Retorna un objeto cortesia destreza por el codigo de barras.
        /// </summary>
        /// <param name="CodigoBarras"></param>
        /// <returns></returns>
        public CortesiaDestreza ObtenerPorCodigoBarras(string CodigoBarras)
        {

            var objCortesiaDestreza  = _cnn.Query<CortesiaDestreza>("SP_ConsultaCodigoBarrasCortesiaDestreza", param: new
            {
                CodigoBarras = CodigoBarras

            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();            

            return objCortesiaDestreza;

        }

        #endregion
    }
}
