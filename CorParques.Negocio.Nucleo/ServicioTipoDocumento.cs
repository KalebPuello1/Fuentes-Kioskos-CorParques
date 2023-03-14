using CorParques.Negocio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Negocio.Entidades;
using CorParques.Datos.Contratos;
using CorParques.Transversales.Util;

namespace CorParques.Negocio.Nucleo
{

	public class ServicioTipoDocumento : IServicioTipoDocumento
	{

		private readonly IRepositorioTipoDocumento _repositorio;

        #region Constructor

        public ServicioTipoDocumento (IRepositorioTipoDocumento repositorio)
		{
			_repositorio = repositorio;
		}


        #endregion

        #region Metodos

        public bool Actualizar(TipoDocumento modelo)
        {
            throw new NotImplementedException();
        }

        public TipoDocumento Crear(TipoDocumento modelo)
        {
            throw new NotImplementedException();
        }

        public TipoDocumento Obtener(int id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// RDSH: Retorna la lista de tipos documento para cargar un combo.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerTipoDocumento()
        {
            return _repositorio.ObtenerLista().Where(x => x.IdEstado == (int)Enumerador.Estados.Activo).Select(x => new TipoGeneral { Id = x.Id, Nombre = x.Nombre }).OrderBy(x=> x.Nombre);
        }

        public IEnumerable<TipoDocumento> ObtenerTodos()
        {
            throw new NotImplementedException();            
        }

        #endregion

    }
}
