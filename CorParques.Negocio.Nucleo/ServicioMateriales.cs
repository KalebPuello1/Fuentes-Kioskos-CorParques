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

	public class ServicioMateriales : IServicioMateriales
	{

		private readonly IRepositorioMateriales _repositorio;

		#region Constructor
		public ServicioMateriales (IRepositorioMateriales repositorio)
		{

			_repositorio = repositorio;
		}

        public bool Actualizar(Materiales modelo)
        {
            throw new NotImplementedException();
        }

        public Materiales Crear(Materiales modelo)
        {
            throw new NotImplementedException();
        }

        public Materiales Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Materiales> ObtenerMaterialesxPuntos(int IdPunto, DateTime? Fecha = null)
        {
            return _repositorio.ObtenerMaterialesxPuntos(IdPunto, Fecha);
        }

        public IEnumerable<Materiales> ObtenerTodos()
        {
            return _repositorio.ObtenerTodos();
        }



        #endregion
    }
}
