using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    /// <summary>
    /// NMSR Taquilla-POS
    /// </summary>
    public interface IRepositorioTipoCliente : IRepositorioBase<TipoCliente>
	{

        IEnumerable<TipoGeneral> ObtenerListaSimple();
    }
}
