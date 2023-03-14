using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;

namespace CorParques.Negocio.Nucleo
{
    /// <summary>
    /// NMSR Configuración Perfiles-Menu
    /// </summary>
    public class ServicioMenu : IServicioMenu
    {

        private readonly IRepositorioMenu _repositorio;

        #region Constructor
        public ServicioMenu(IRepositorioMenu repositorio)
        {

            _repositorio = repositorio;
        }
        #endregion
        public bool Actualizar(Menu modelo)
        {
            throw new NotImplementedException();
        }

        public Menu Crear(Menu modelo)
        {
            throw new NotImplementedException();
        }

        public Menu Obtener(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna todas los Menus activos.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Menu> ObtenerListaActivos()
        {
            IEnumerable<Menu> listaMenus = new List<Menu>();
            listaMenus = _repositorio.ObtenerListaActivos();

            List<Menu> menusPrincipales = listaMenus.OrderBy(o => o.Orden).Where(o => !o.IdPadre.HasValue).ToList();
            foreach (Menu menu in menusPrincipales)
                AgregarMenuHijo(menu, listaMenus);

            return menusPrincipales;
        }

        public IEnumerable<Menu> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Llena la colección de menus hijos de manera recursiva
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="listaMenus"></param>
        private void AgregarMenuHijo(Menu menu, IEnumerable<Menu> listaMenus)
        {
            IEnumerable<Menu> menus = listaMenus.Where(o => o.IdPadre == menu.IdMenu).ToList();
            menu.MenuHijos = menus;
            foreach (Menu menuHijo in menu.MenuHijos)
            {
                List<Menu> menusHijos = listaMenus.Where(o => o.IdPadre == menuHijo.IdMenu).ToList();
                menuHijo.MenuHijos = menusHijos;
            }
        }
    }
}
