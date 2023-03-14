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
    /// NMSR CU006-DE-1CI-ARS-001-Configuración centro médico
    /// </summary>
    public class ServicioCentroMedico : IServicioCentroMedico
	{

		private readonly IRepositorioCentroMedico _repositorio;

		#region Constructor

		public ServicioCentroMedico (IRepositorioCentroMedico repositorio)
		{

			_repositorio = repositorio;
		}

        #endregion

        #region Metodos

        public bool Insertar(CentroMedico modelo, out string error)
        {
            return _repositorio.Insertar(modelo, out error);
        }

        public bool Actualizar(CentroMedico modelo, out string error)
        {
            return _repositorio.Actualizar(modelo, out error);
        }

        //public IEnumerable<CentroMedico> ObtenerListaActivos()
        //{
        //    return _repositorio.ObtenerListaActivos();
        //}

        public CentroMedico Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }        

        public IEnumerable<CentroMedico> ObtenerTodos()
        {
            return _repositorio.ObtenerListaActivos();
        }

        public IEnumerable<TipoGeneral> ObtenerListaCentroMedico()
        {
            return _repositorio.ObtenerListaCentroMedico();
        }


        public CentroMedico Crear(CentroMedico modelo)
        {
            throw new NotImplementedException();
        }

        public bool Actualizar(CentroMedico modelo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// RDSH: Retorna la lista de zona area o de ubicaciones para el reporte de centro medico.
        /// </summary>
        /// <param name="intIdCentroMedico"></param>
        /// <returns></returns>
        public IEnumerable<TipoGeneral> ObtenerListaZonaAreaUbicacion(int intIdCentroMedico)
        {
            return _repositorio.ObtenerListaZonaAreaUbicacion(intIdCentroMedico);
        }

        public IEnumerable<TipoGeneral> ObtenerZonas()
        {
            return _repositorio.ObtenerZonas();
        }

        #endregion
    }
}
