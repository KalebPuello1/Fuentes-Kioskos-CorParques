using CorParques.Negocio.Entidades;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Datos.Contratos
{
    /// <summary>
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
	public interface IRepositorioMenu : IRepositorioBase<Menu>
	{

        /// <summary>
        /// Retorna todas los Menus activos.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Menu> ObtenerListaActivos();

    }
}
