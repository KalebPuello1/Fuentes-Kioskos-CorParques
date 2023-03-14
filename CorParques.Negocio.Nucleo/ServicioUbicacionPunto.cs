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

	public class ServicioUbicacionPunto : IServicioUbicacionPunto
	{

		private readonly IRepositorioUbicacionPunto _repositorio;

		#region Constructor
		public ServicioUbicacionPunto (IRepositorioUbicacionPunto repositorio)
		{

			_repositorio = repositorio;
		}

        #endregion

        #region Metodos

        public bool Actualizar(UbicacionPunto modelo)
        {
            return _repositorio.Actualizar(ref modelo);
        }

        public UbicacionPunto Crear(UbicacionPunto modelo)
        {
            UbicacionPunto objUbicacionPunto = null;

            try
            {
                modelo.IdUsuarioModificacion = null;
                modelo.FechaModificacion = null;
                modelo.IdUbicacionPunto = _repositorio.Insertar(ref modelo);
                if (modelo.IdUbicacionPunto > 0)
                    objUbicacionPunto = modelo;
            }
            catch (Exception ex)
            {
                objUbicacionPunto = null;
                throw new ArgumentException(string.Concat("Error en Crear_ServicioUbicacionPunto: ", ex.Message));
            }

            return objUbicacionPunto;

        }

        public bool ActualizarUbicacion(UbicacionPunto modelo, out string error)
        {
            return _repositorio.ActualizarUbicacion(modelo, out error);
        }
        public bool Insertar(UbicacionPunto modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public UbicacionPunto Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<UbicacionPunto> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        /// <summary>
        ///RDSH: Retorna todas las ubicaciones activas para un punto especifico.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public IEnumerable<UbicacionPunto> ObtenerPorPunto(int intIdPunto)
        {
            return _repositorio.ObtenerLista().Where(x => x.IdPunto == intIdPunto && x.IdEstado == (int)Transversales.Util.Enumerador.Estados.Activo);
        }

        /// <summary>
        /// RDSH: Retorna una lista de tipo general para cargar un dropdown list.
        /// </summary>
        /// <param name="intIdPunto"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaSimplePorPunto(int intIdPunto)
        {
            return _repositorio.ObtenerLista().Where(x => x.IdPunto == intIdPunto && x.IdEstado == (int)Transversales.Util.Enumerador.Estados.Activo).Select(x=> new TipoGeneral {Id = x.IdUbicacionPunto, Nombre = x.Nombre}).OrderBy(x => x.Nombre);
        }

        public bool Eliminar(int id)
        {
            return _repositorio.EliminarLogica(id);
        }

        #endregion

    }
}
