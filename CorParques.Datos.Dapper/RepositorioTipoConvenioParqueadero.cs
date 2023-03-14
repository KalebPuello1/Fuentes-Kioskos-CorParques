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

    public class RepositorioTipoConvenioParqueadero : RepositorioBase<TipoConvenioParqueadero>, IRepositorioTipoConvenioParqueadero
    {

        /// <summary>
        /// RDSH: Obtiene la lista de convenios activos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _cnn.GetList<TipoConvenioParqueadero>().Where(x => x.IdEstado == 1).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
        }
    }
}
