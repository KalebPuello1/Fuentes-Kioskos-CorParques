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

	public class ServicioArea : IServicioArea
	{

		private readonly IRepositorioArea _repositorio;

		#region Constructor

		public ServicioArea (IRepositorioArea repositorio)
		{
			_repositorio = repositorio;
		}

        #endregion

        #region Metodos

        public bool Actualizar(Area modelo)
        {
            return _repositorio.Actualizar(ref modelo);
        }

        public Area Crear(Area modelo)
        {
            _repositorio.Insertar(ref modelo);
            return modelo;
        }

        public Area Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<TipoGeneral> ObtenerListaAreas()
        {
            return _repositorio.ObtenerListaAreas();
        }

        public IEnumerable<Area> ObtenerTodos()
        {
            //return _repositorio.ObtenerLista().Where(x => x.IdEstado == (int)Enumerador.Estados.Activo).ToList();
            return null;
        }
        #endregion


        
    }
}
