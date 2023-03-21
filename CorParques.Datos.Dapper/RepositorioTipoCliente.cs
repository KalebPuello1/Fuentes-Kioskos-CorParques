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
    /// <summary>
    /// NMSR Taquilla-POS
    /// </summary>
	public class RepositorioTipoCliente : RepositorioBase<TipoCliente>,  IRepositorioTipoCliente
	{
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _cnn.GetList<TipoCliente>().Select(x => new TipoGeneral { Id = x.IdTipoCliente, Nombre = x.Nombre });
        }

    }
}
