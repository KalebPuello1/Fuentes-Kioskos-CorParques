using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Contratos
{
    /// <summary>
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
	public interface IServicioMenu : IServicioBase<Menu>
	{

        /// <summary>
        /// Retorna todas los Menus activos.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Menu> ObtenerListaActivos();
    }
}
