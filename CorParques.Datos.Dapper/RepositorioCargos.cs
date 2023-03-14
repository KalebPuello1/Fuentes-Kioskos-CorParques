using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{
    /// <summary>
    /// KADM Configuración Cargos
    /// </summary>
	public class RepositorioCargos : IRepositorioCargos
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioCargos()
        {
            _cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[string.Format("{0}-{1}", ConfigurationManager.AppSettings["CnnAmbiente"], ConfigurationManager.AppSettings["Cnn"])].ConnectionString);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            var lista = _cnn.GetList<Cargos>().Select(x => new TipoGeneral { Id = x.IdCargo, Nombre = x.Nombre });
            return lista;
        }


      
        public bool Insertar(Cargos modelo, out string error)
        {
            object param = new
            {
                IdCargo= modelo.IdCargo,
                Nombre = modelo.Nombre,
                IdEstado = modelo.IdEstado,
                UsuarioCreacion = modelo.UsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion,
                
            };
            error = ExecuteStoreProcedure(modelo,param, "dbo.SP_ConsultarCargo");
            return string.IsNullOrEmpty(error);
        }

        
        public bool GuardarCargoPerfil(Cargos modelo, out string error)
        {
            object param = new
            {
                IdCargo = modelo.IdCargo,
                Nombre = modelo.Nombre,
                IdEstado = modelo.IdEstado,
                UsuarioCreacion = modelo.UsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion,
            };
            error = ExecuteStoreProcedure(modelo, param, "dbo.SP_GuardarCargoPerfil");
            return string.IsNullOrEmpty(error);
        }


     
        private string ExecuteStoreProcedure(Cargos modelo, object param,string storeProcedure)
        {
           
            string error = _cnn.Query<string>(storeProcedure, param, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return error;
        }

        public Cargos ObtenerxId(int IdCargo)
        {
            try
            {
                Cargos rta = _cnn.GetList<Cargos>().Where(x => x.IdCargo == IdCargo).FirstOrDefault();
                
                return rta;
            }
            catch (Exception)
            {
                return null;
            }
        }
      
        public bool Actualizar(Cargos cargos)
        {
            try
            {                
                _cnn.Query("DELETE FROM [sec].[TB_CargoPerfil] WHERE IdCargo =" + cargos.IdCargo + "");
                foreach (Perfil item in cargos.Perfiles)
                {
                    CargosPerfil cargosPerfil = new CargosPerfil();
                    cargosPerfil.IdCargo = cargos.IdCargo;
                    cargosPerfil.IdPerfil = item.IdPerfil;
                    _cnn.Insert(cargosPerfil);
                }

            }
            catch (Exception ex)
            {

                Utilidades.RegistrarError(ex, "Actualizar_Cargos");                
                throw;
            }


            return true;

        }


        public IEnumerable<Cargos> ConsultarCargo(Cargos model)
        {
            var Listaperfil = _cnn.Query<Cargos>("dbo.SP_ConsultarCargoxID", param: new
            {
                IdCargo = model.IdCargo
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return Listaperfil;
        }

        public IEnumerable<Cargos> ObtenerLista()
        {
            var lista = _cnn.GetList<Cargos>().Where(x => x.IdEstado == 1);
            return lista;
        }
        
        public IEnumerable<CargosPerfil> ObtenerListaCargoPerfil(int idCargo)
        {
            var lista = _cnn.GetList<CargosPerfil>().Where(x => x.IdCargo == idCargo);
            return lista;
        }


    }
}
