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
    public class ServicioTipoBrazalete : IServicioTipoBrazalete
    {
        private readonly IRepositorioTipoBrazalete _repositorio;

        private readonly IRepositorioPuntos _repositorioAtracciones;

        private readonly IRepositorioEstados _repositorioEstado;

        public ServicioTipoBrazalete(IRepositorioTipoBrazalete repositorio, IRepositorioPuntos repositorioAtracciones, IRepositorioEstados repositorioEstado)
        {
            _repositorio = repositorio;
            _repositorioAtracciones = repositorioAtracciones;
            _repositorioEstado = repositorioEstado;
        }

        public bool ActualizarBrazalete(TipoBrazalete Modelo)
        {
            return _repositorio.ActualizarBrazalete(Modelo);             
        }

        public IEnumerable<Puntos> ObtenerAtraccionxBrazalete(int idTipoBrazalete)
        {
            return _repositorio.ObtenerAtraccionxBrazalete(idTipoBrazalete);
        }

        public IEnumerable<TipoBrazalete> ObtenerTodosBrazalete()
        {
            return _repositorio.ObtenerTodosBrazalete();
        }

        public IEnumerable<TipoBrazalete> ObtenerBrazaletesSupervisor(int supervisor)
        {
            return _repositorio.ObtenerBrazaletesSupervisor(supervisor);
        }

        public bool BorrarTipoBrazalete(int idTipoBrazalete)
        {
            return _repositorio.BorrarTipoBrazalete(idTipoBrazalete);
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ObtenerListaSimple();
        }

       public IEnumerable<TipoGeneral> ObtenerListaSimpleAtraccion ()
        {
            return _repositorioAtracciones.ObtenerListaSimple();
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimpleEstado()
        {
            return _repositorioEstado.ObtenerEstados(1);
        }

        public TipoBrazalete Obtener(int id)
        {
            return _repositorio.ObtererBrazalete(id);
        }

        public bool Desactivar(int id)
        {
            var TipoBrazalete = _repositorio.ObtererBrazalete(id);
            TipoBrazalete.IdEstado = (int)Enumerador.Estados.Inactivo;
            return _repositorio.ActualizarBrazalete(TipoBrazalete);

        }

        public IEnumerable<TipoBrazalete> ObtenerTodosBrazaleteInventario(int IdPunto)
        {
            return _repositorio.ObtenerTodosBrazaleteInventario(IdPunto);
        }
    }
}
