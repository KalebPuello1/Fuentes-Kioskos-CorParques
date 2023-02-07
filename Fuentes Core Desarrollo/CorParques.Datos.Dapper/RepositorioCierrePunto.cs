using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using CorParques.Transversales.Util;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Dapper
{

	public class RepositorioCierrePunto : RepositorioBase<CierreElementos>,  IRepositorioCierrePunto
	{
        public IEnumerable<CierreElementos> ObtenerElementosCierre(int IdPunto)
        {

            var lista = _cnn.Query<CierreElementos>("SP_ObtenerAperturaElementos", param: new { IdPunto = IdPunto }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            var listaEstado = _cnn.Query<Estados>("SP_ObtenerEstadosPorModulo", param: new
            {
                IdModulo = (int)Enumerador.ModulosAplicacion.CierreElementos
            }, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });            

            foreach (CierreElementos item in lista)
            { 
                item.Elemento = _cnn.Get<TipoElementos>(item.IdElemento);
                item.Estados = listaEstado;
            }
            return lista;
        }

        public IEnumerable<CierreElementos> ObtenerElementosCierreSupervisor(int Usuario)
        {

            var lista = _cnn.Query<CierreElementos>("SP_ObtenerAperturaElementosUsuario", param: new { IdUsuario = Usuario, Tipo = 1 }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            var listaEstado = _cnn.Query<Estados>("SP_ObtenerEstadosPorModulo", param: new
            {
                IdModulo = (int)Enumerador.ModulosAplicacion.CierreElementos
            }, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });

            foreach (CierreElementos item in lista)
            {
                item.Elemento = _cnn.Get<TipoElementos>(item.IdElemento);
                item.Estados = listaEstado;
            }
            return lista;
        }

        public IEnumerable<CierreElementos> ObtenerElementosCierreNido(int Usuario)
        {

            var lista = _cnn.Query<CierreElementos>("SP_ObtenerAperturaElementosUsuario", param: new { IdUsuario = Usuario, Tipo = 2 }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            var listaEstado = _cnn.Query<Estados>("SP_ObtenerEstadosPorModulo", param: new
            {
                IdModulo = (int)Enumerador.ModulosAplicacion.CierreElementos
            }, commandType: System.Data.CommandType.StoredProcedure).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });

            foreach (CierreElementos item in lista)
            {
                item.Elemento = _cnn.Get<TipoElementos>(item.IdElemento);
                item.Estados = listaEstado;
            }
            return lista;
        }

        #region Metodos

        /// <summary>
        /// RDSH: Actualiza un cierre de elementos para supervisor o para nido.
        /// </summary>
        /// <param name="intTipo"></param>
        /// <param name="modelo"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool ActualizarMasivo(int intTipo, IEnumerable<CierreElementos> modelo, out string error)
        {
            try
            {

                if (intTipo == 1)
                {
                    //RDSH: Actualiza datos supervisor.
                    error = _cnn.Query<string>("SP_ActualizarCierreElementos", param: new
                    {
                        Tipo = intTipo,
                        DetalleCierreElementos = Utilidades.convertTable(modelo
                                                       .Select(x => new TablaGeneral
                                                       {
                                                           col1 = x.Id.ToString(),
                                                           col2 = x.IdEstadoSupervisor.ToString(),
                                                           col6 = x.ObservacionesSupervisor.ToString(),
                                                           col4 = x.IdUsuarioSupervisor.ToString()
                                                       })),
                    }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                }
                else
                {
                    //RDSH: Actualiza datos nido.
                    error = _cnn.Query<string>("SP_ActualizarCierreElementos", param: new
                    {
                        Tipo = intTipo,
                        DetalleCierreElementos = Utilidades.convertTable(modelo
                                                       .Select(x => new TablaGeneral
                                                       {
                                                           col1 = x.Id.ToString(),
                                                           col2 = x.IdEstadoNido.ToString(),
                                                           col6 = x.ObservacionesNido.ToString(),
                                                           col4 = x.IdUsuarioNido.ToString()
                                                       })),
                    }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                }

               
            }
            catch (Exception ex)
            {
                error = string.Concat("Error en ActualizarMasivo_RepositorioCierrePunto", ex.Message);
            }

            return string.IsNullOrEmpty(error);
        }

        #endregion
    }
}
