using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioCargueBoleteria : RepositorioBase<CargueBoleteria>,  IRepositorioCargueBoleteria
	{

        #region Metodos

        /// <summary>
        /// RDSH: Inserta un cargue masivo de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(CargueBoleteria modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarCargueBoleteria", param: new
                {
                    IdProducto = modelo.IdProducto,
                    Descripcion = modelo.Descripcion,
                    ConsecutivoInicial = modelo.ConsecutivoInicial,
                    ConsecutivoFinal =  modelo.ConsecutivoFinal,                    
                    IdEstado = modelo.IdEstado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion,                    
                    FechaInicioVigencia = modelo.FechaInicioVigencia,
                    FechaFinVigencia = modelo.FechaFinVigencia                  

                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioCargueBoleteria: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Actualiza el estado del cargue de boleteria.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(CargueBoleteria modelo, out string error)
        {

            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_ActualizarCargueBoleteria", param: new
                {
                    IdCargueBoleteria = modelo.IdCargueBoleteria,
                    IdEstado = modelo.IdEstado,                    
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioCargueBoleteria: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH: Retorna la lista de cargues realizados en la tabla TB_CargueBoleteria.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CargueBoleteria> ObtenerListaCargueBoleteria()
        {

            var lista = _cnn.Query<CargueBoleteria>("SP_ObtenerCargueBoleteria", null, commandType: System.Data.CommandType.StoredProcedure).ToList();            
            return lista;
        }

        /// <summary>
        /// RDSH: Retorna los productos de tipo boleteria.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTipoBoleteria()
        {                     
            var lista = _cnn.Query<TipoBrazalete>("ObtenerTiposBoleteria", null, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
            return lista;
        }

        #endregion

    }
}
