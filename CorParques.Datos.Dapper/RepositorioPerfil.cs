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
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
	public class RepositorioPerfil : IRepositorioPerfil
    {
        #region Declaraciones

        private readonly SqlConnection _cnn;

        #endregion

        #region Constructor

        public RepositorioPerfil()
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
            var lista = _cnn.GetList<Perfil>().Where(x => x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.IdPerfil, Nombre = x.Nombre });
            return lista;
        }


        /// <summary>
        /// Insertar un perfil en la tabla TB_Perfil
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Insertar(Perfil modelo, out string error)
        {
            object param = new
            {
                Nombre = modelo.Nombre,
                IdEstado = modelo.IdEstado,
                IdUsuarioCreacion = modelo.IdUsuarioCreacion,
                FechaCreacion = modelo.FechaCreacion,
                IdsMenus = string.Join(",", modelo.Menus.Select(m => m.IdMenu))
            };
            error = ExecuteStoreProcedure(modelo,param, "sec.SP_InsertarPerfil");
            return string.IsNullOrEmpty(error);
        }

        /// <summary>
        /// Actualiza un perfil en la tabla TB_Perfil
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Actualizar(Perfil modelo, out string error)
        {
            object param = new
            {
                IdPerfil=modelo.IdPerfil,
                Nombre = modelo.Nombre,
                IdEstado = modelo.IdEstado,
                IdUsuarioModificacion = modelo.IdUsuarioModificacion,
                FechaModificacion = modelo.FechaModificacion,
                IdsMenus = string.Join(",", modelo.Menus.Select(m => m.IdMenu))
            };
            error = ExecuteStoreProcedure(modelo, param,"sec.SP_ActualizarPerfil");
            return string.IsNullOrEmpty(error);
        }

       

        /// <summary>
        /// Ejecuta un store procedure retornando un string desde el SP
        /// </summary>
        /// <param name="modelo"></param>
        /// <param name="storeProcedure"></param>
        /// <returns></returns>
        private string ExecuteStoreProcedure(Perfil modelo, object param,string storeProcedure)
        {
           
            string error = _cnn.Query<string>(storeProcedure, param, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return error;
        }

        /// <summary>
        /// Retorna un Perfil por id.
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <returns></returns>
        public Perfil Obtener(int idPerfil)
        {
            try
            {
                var rta = _cnn.QueryMultiple("sec.SP_ConsultarPerfilId", new { IdPerfil = idPerfil }, commandType: System.Data.CommandType.StoredProcedure);
                var perfil = rta.Read<Perfil>().Single();
                if (perfil != null)
                    perfil.Menus = rta.Read<Menu>().ToList();
                    return perfil;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

         public string ActualizarSegregacion(SegregacionFunciones model )
        {

            string error = string.Empty;

            object param = new
            {
                IdPerfil = model.IdPerfil,
                SegregacionFunciones = Utilidades.convertTable(model.ListaPerfilConflicto
                .Select(x => new TablaGeneral
                {
                    col1 = x.IdPerfil.ToString(),
                    col2 = x.IdUsuarioCreacion.ToString()
                })),

            };

            error = _cnn.Query<string>("sec.SP_ActualizarSegregacionFunciones", param, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            return error;

           
            //var rta = _cnn.Query<int>(,
            //           commandType: CommandType.StoredProcedure,
            //           param: new
            //           {
            //               IdPerfil = model.IdPerfil,

            //               SegregacionFunciones = Utilidades.convertTable(model.ListaPerfilConflicto
            //                                            .Select(x => new TablaGeneral
            //                                            {
            //                                                col1 = x.IdPerfil.ToString(),
            //                                                col2 = x.IdUsuarioCreacion.ToString()
            //                                            })),
                         
            //           });

            //return rta.First().ToString();
        }

        public IEnumerable<Perfil> ConsultarSegregacion(int idPerfil)
        {
            var Listaperfil = _cnn.Query<Perfil>("sec.SP_ConsultarSegregacionFunciones", param: new
            {
                IdPerfil = idPerfil
            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            return Listaperfil;
        }

            /// <summary>
            /// Lista los perfiles con todos los estado para la lista inicial
            /// </summary>
            /// <returns></returns>
        public IEnumerable<Perfil> ObtenerLista()
        {
            var lista = _cnn.GetList<Perfil>();
            return lista;
        }

        public IEnumerable<Perfil> PerfilActivos(int IdPerfilActual)
        {
            var lista = _cnn.GetList<Perfil>().Where(x => x.IdEstado==1  && x.IdPerfil != IdPerfilActual).ToList();
            return lista;
        }
        /// <summary>
        /// Actualiza el modelo enviando el estado inactivo desde el controlador
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        public bool Inactivar(Perfil modelo)
        {
            return _cnn.Update(modelo) > 0;
        }

    }
}
