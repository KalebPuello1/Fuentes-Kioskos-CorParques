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

	public class RepositorioUbicacionPunto : RepositorioBase<UbicacionPunto>,  IRepositorioUbicacionPunto
	{
        #region Declaraciones

        private readonly SqlConnection _conn;

        #endregion

        #region Constructor

        public RepositorioUbicacionPunto()
        {
            _conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        #endregion
        public bool Insertar(UbicacionPunto modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _conn.Query<string>("SP_InsertarUbicacionPunto", param: new
                {
                    IdPunto = modelo.IdPunto,
                    Nombre = modelo.Nombre,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                    FechaCreacion = modelo.FechaCreacion,
                    RequiereAuxiliar = modelo.RequiereAuxiliar
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Insertar_RepositorioUbicacionPunto: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        public bool ActualizarUbicacion(UbicacionPunto modelo, out string error)
        {
            error = string.Empty;

            try
            {
                error = _conn.Query<string>("SP_ActualizarUbicacionPunto", param: new
                {
                    IdPunto = modelo.IdPunto,
                    IdUbicacionPunto = modelo.IdUbicacionPunto,
                    Nombre = modelo.Nombre,
                    IdEstado = modelo.IdEstado,
                    IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                    FechaModificacion = modelo.FechaModificacion,
                    RequiereAuxiliar = modelo.RequiereAuxiliar
                }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                error = string.Concat("Error inesperado en Actualizar_RepositorioUbicacionPunto: ", ex.Message);
            }

            return string.IsNullOrEmpty(error);

        }

        public bool EliminarLogica(int id)
        {
            var item = _cnn.Get<UbicacionPunto>(id);
            item.IdEstado = 2;
            return _cnn.Update(item) > 0;
        }
    }
}
