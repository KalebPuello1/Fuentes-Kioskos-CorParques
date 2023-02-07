using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Transversales.Util;

namespace CorParques.Datos.Dapper
{
    public class RepositorioParametros : RepositorioBase<Parametro>, IRepositorioParametros
    {
        public Parametro ObtenerParametroPorNombre(string strNombre)
        {
            return _cnn.GetList<Parametro>().Where(x => x.Nombre == strNombre).FirstOrDefault();
        }

        /// <summary>
        /// RDSH: Consulta todos los parametros de la aplicacion para almacenarlos en cache.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parametro> ObtenerParametrosGlobales()
        {
            
            try
            {
                var objListaParametros = _cnn.Query<Parametro>("SP_ObtenerParametrosGlobales", null, commandType: System.Data.CommandType.StoredProcedure).ToList();
                return objListaParametros;
            }
            catch (Exception ex)
            {
                Utilidades.RegistrarError(ex, "RepositorioParametros_ObtenerParametrosGlobales");
                return null;
            }
        }

    }
}
