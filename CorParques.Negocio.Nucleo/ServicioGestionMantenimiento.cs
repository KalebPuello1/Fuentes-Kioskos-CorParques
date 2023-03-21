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
    public class ServicioGestionMantenimiento : IServicioGestionMantenimiento
    {
        private readonly IRepositorioGestionMantenimiento _repositorio;

        
        public ServicioGestionMantenimiento(IRepositorioGestionMantenimiento gestion)
        {
            _repositorio = gestion;
        }

        public bool Actualizar(GestionMantenimiento modelo)
        {
            throw new NotImplementedException();
        }

        public GestionMantenimiento Crear(GestionMantenimiento modelo)
        {
            throw new NotImplementedException();
        }

        public GestionMantenimiento Obtener(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GestionMantenimiento> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TipoGeneral> ObtenerxAtraccion(int id)
        {
            return _repositorio.ObtenerListaSimple(id);
        }


        //public IEnumerable<Parametro> ObtenerTodos()
        //{
        //    return _repositorio.ObtenerLista();
        //}
    }
}
