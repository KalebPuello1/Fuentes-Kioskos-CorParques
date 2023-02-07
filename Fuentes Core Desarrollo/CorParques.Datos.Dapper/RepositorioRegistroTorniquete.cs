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

	public class RepositorioRegistroTorniquete : RepositorioBase<RegistroTorniquete>,  IRepositorioRegistroTorniquete
	{

        #region Metodos

        /// <summary>
        /// RDSH:Actualiza el registro de torniquete y remueve a los auxiliares asociados al punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(RegistroTorniquete modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_ActualizarRegistroTorniquete", param: new
                {
                    IdRegistroTorniquete = modelo.IdRegistroTorniquete,
                    IdPunto = modelo.IdPunto,
                    Inicio = modelo.Inicio,
                    Fin = modelo.Fin,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Actualizar_RepositorioRegistroTorniquete: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RDSH:Actualiza el registro de torniquete y remueve a los auxiliares asociados al punto.
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(RegistroTorniquete modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _cnn.Query<string>("SP_InsertarRegistroTorniquete", param: new
                {
                    IdPunto = modelo.IdPunto,
                    Inicio = modelo.Inicio,
                    Fin = modelo.Fin,                    
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en Insertar_RepositorioRegistroTorniquete: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// RSDH: Retorna un objeto torniquete con la informacion de del dia o del ultimo registro.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <param name="blnDia"></param>
        /// <returns></returns>
        public RegistroTorniquete ObtenerRegistroTorniquete(int intIdPunto)
        {
            var objRegistroTorniquete = _cnn.Query<RegistroTorniquete>("SP_ObtenerRegistroTorniquete", param: new
            {
                IdPunto = intIdPunto                
            }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return objRegistroTorniquete;
        }

        #endregion

    }
}
