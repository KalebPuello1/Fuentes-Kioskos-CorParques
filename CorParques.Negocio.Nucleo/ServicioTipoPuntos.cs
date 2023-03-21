using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioTipoPuntos : IServicioTipoPuntos
    {
        protected IRepositorioTipoPuntos _repositorio;

        public ServicioTipoPuntos(IRepositorioTipoPuntos puntoVenta)
        {
            this._repositorio = puntoVenta;
        }

        public bool Actualizar(TipoPuntos modelo)
        {
            throw new NotImplementedException();
        }

        public TipoPuntos Crear(TipoPuntos modelo)
        {
            throw new NotImplementedException();
        }

        public TipoPuntos Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple() ;
        }

        public IEnumerable<TipoPuntos> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
