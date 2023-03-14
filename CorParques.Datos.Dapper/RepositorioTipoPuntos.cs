using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Entidades;
using Dapper;

namespace CorParques.Datos.Dapper
{
    public class RepositorioTipoPuntos : RepositorioBase<TipoPuntos>, IRepositorioTipoPuntos
    {
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            var lista = _cnn.GetList<TipoPuntos>().Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre });
            return lista;
        }
    }
}
