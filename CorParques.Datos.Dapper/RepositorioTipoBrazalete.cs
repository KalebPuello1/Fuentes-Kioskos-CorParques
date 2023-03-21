using CorParques.Datos.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioTipoBrazalete : RepositorioBase<TipoBrazalete>,  IRepositorioTipoBrazalete
    {
        private readonly SqlConnection _cnn;
        public RepositorioTipoBrazalete()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        public bool ActualizarBrazalete(TipoBrazalete Modelo)
        {

            try
            {
                _cnn.Query<bool>("Actualizar_Tipo_Brazalete",
                param: new { IdTipoBrazalete = Modelo.Id, Nombre = Modelo.Nombre, Descripcion = Modelo.Descripcion, IdEstado = Modelo.IdEstado ,UsuarioCreacion = Modelo.UsuarioCreacion, FechaCreacion = Modelo.FechaCreacion, UsuarioModificacion = Modelo.UsuarioCreacion, FechaModificacion = Modelo.FechaModificacion, Precio = Modelo.Precio, IdAtraccion = Modelo.AtraccionesSeleccionadas},
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            var lista = _cnn.GetList<TipoBrazalete>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
            return lista;
        }

        public IEnumerable<Puntos> ObtenerAtraccionxBrazalete(int idTipoBrazalete)
        {

            return _cnn.Query<Puntos>("Retornar_AtraccionXBrazalete", param: new { idTipoBrazalete = idTipoBrazalete }, commandType: System.Data.CommandType.StoredProcedure).ToList();

        }

        public IEnumerable<TipoBrazalete> ObtenerTodosBrazalete()
        {
            return _cnn.Query<TipoBrazalete>("ObtenerTiposBrazalete", commandType: System.Data.CommandType.StoredProcedure).ToList();
        }
        public IEnumerable<TipoBrazalete> ObtenerBrazaletesSupervisor(int supervisor)
        {
            return _cnn.Query<TipoBrazalete>("ObtenerBrazaletesSupervisor", param: new { idSupervisor = supervisor }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        public IEnumerable<TipoBrazalete> ObtenerTodosBrazaleteInventario(int IdPunto)
        {
            return _cnn.Query<TipoBrazalete>("SP_ObtenerTiposBrazaleteInventario", param: new { IdPunto = IdPunto }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        }

        public bool BorrarTipoBrazalete(int idTipoBrazalete)
        {
            try
            {
                _cnn.Query<bool>("Borrar_Tipo_Brazalete", param: new { idTipoBrazalete = idTipoBrazalete }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public TipoBrazalete ObtererBrazalete(int idTipoBrazalete)
        {
            return _cnn.Query<TipoBrazalete>("Retornar_BrazaletexId", param: new { idTipoBrazalete = idTipoBrazalete }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
         }

    }
}
