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
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
	public class RepositorioMenu : RepositorioBase<Menu>,  IRepositorioMenu
	{

        /// <summary>
        /// Retorna todas los Menus activos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Menu> ObtenerListaActivos()
        {
            return _cnn.GetList<Menu>().Where(x => x.IdEstado == 1);
        }
    }
}
