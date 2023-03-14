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
    public class RepositorioPasaporteUso : RepositorioBase<PasaporteUso>,  IRepositorioPasaporteUso
    {
        private readonly SqlConnection _cnn;

        public RepositorioPasaporteUso()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["CnnAmbiente"] + "-" + ConfigurationManager.AppSettings["Cnn"]].ConnectionString);
        }

        public bool ActualizarPasaporteUso(PasaporteUso Modelo)
        {

            try
            {
                _cnn.Query<bool>("SP_ActualizarPasaporteUso",
                param: new { IdPasaporteUso = Modelo.Id, Nombre = Modelo.Nombre, Descripcion = Modelo.Descripcion, IdEstado = Modelo.IdEstado ,UsuarioCreacion = Modelo.UsuarioCreacion, FechaCreacion = Modelo.FechaCreacion, UsuarioModificacion = Modelo.UsuarioCreacion, FechaModificacion = Modelo.FechaModificacion, Precio = Modelo.Precio, IdAtraccion = Modelo.AtraccionesSeleccionadas},
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
            var lista = _cnn.GetList<PasaporteUso>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
            return lista;
        }


        //public IEnumerable<PasaporteUso> ObtenerTodosPasaporte()
        //{
        //    return _cnn.Query<PasaporteUso>("SP_RetornarTodosPasaporteUso", commandType: System.Data.CommandType.StoredProcedure).ToList();
        //}


        public PasaporteUso ObtenerPasaporte(int idTipoBrazalete)
        {
            return _cnn.Query<PasaporteUso>("SP_RetornarPasaportexId", param: new { IdPasaporteUso = idTipoBrazalete }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
         }

    }
}
