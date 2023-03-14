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

	public class ServicioResolucionFactura : IServicioResolucionFactura
    {

		private readonly IRepositorioResolucionFactura _repositorio;

		#region Constructor

		public ServicioResolucionFactura(IRepositorioResolucionFactura repositorio)
		{
			_repositorio = repositorio;
		}

        #endregion

        #region Metodos

        public bool Actualizar(TipoGeneral modelo)
        {
            throw new NotImplementedException();
        }

        public TipoGeneral Crear(TipoGeneral modelo)
        {
            throw new NotImplementedException();
        }

        public string EliminarResolucion(int id)
        {
            return _repositorio.EliminarResolucion(id);
        }
        public string AprobarResolucion(int id)
        {
            return _repositorio.AprobarResolucion(id);
        }

        public string InsertarResolucion(ResolucionFactura resolucion)
        {
            return _repositorio.InsertarResolucion(resolucion);
        }

        public TipoGeneral Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ResolucionFactura> ObtenerPrefijoConsecutivo(string prefijo, int consecutivoInicial)
        {
            return _repositorio.ObtenerPrefijoConsecutivo(prefijo, consecutivoInicial);
        }

        public IEnumerable<ResolucionFactura> ObtenerResoluciones(int aprovador)
        {
            return _repositorio.ObtenerResoluciones(aprovador);
        }

        public IEnumerable<TipoGeneral> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        #endregion




    }
}
