using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    /// <summary>
    /// NMSR Taquilla-POS
    /// </summary>
    public interface IServicioTipoCliente 
	{

        IEnumerable<TipoGeneral> ObtenerListaSimple();
    }
}
