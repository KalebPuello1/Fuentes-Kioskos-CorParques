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
    public class ServicioPasaporteUso : IServicioPasaporteUso
    {
        private readonly IRepositorioPasaporteUso _repositorio;

        private readonly IRepositorioPuntos _repositorioAtracciones;

        private readonly IRepositorioEstados _repositorioEstado;

        public ServicioPasaporteUso(IRepositorioPasaporteUso repositorio, IRepositorioPuntos repositorioAtracciones, IRepositorioEstados repositorioEstado)
        {
            _repositorio = repositorio;
            _repositorioAtracciones = repositorioAtracciones;
            _repositorioEstado = repositorioEstado;
        }

        public bool ActualizarPasaporteUso(PasaporteUso Modelo)
        {
            return _repositorio.ActualizarPasaporteUso(Modelo);             
        }        

        //public IEnumerable<PasaporteUso> ObtenerTodosPasaporteUso()
        //{
        //    return _repositorio.ObtenerTodosPasaporte();
        //}

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

        public PasaporteUso Obtener(int id)
        {
            return _repositorio.ObtenerPasaporte(id);
        }
    }
}
