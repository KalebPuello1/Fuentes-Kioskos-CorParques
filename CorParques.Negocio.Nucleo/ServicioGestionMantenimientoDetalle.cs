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
    public class ServicioGestionMantenimientoDetalle : IServicioGestionMantenimientoDetalle
    {
        private readonly IRepositorioGestionMantenimientoDetalle _repositorio;

        
        public ServicioGestionMantenimientoDetalle(IRepositorioGestionMantenimientoDetalle gestion)
        {
            _repositorio = gestion;
        }

        public bool Actualizar(GestionMantenimientoDetalle modelo)
        {
            throw new NotImplementedException();
        }

        public GestionMantenimientoDetalle Crear(GestionMantenimientoDetalle modelo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GestionMantenimientoDetalle> ObtenerListaSimple(int id)
        {
            return _repositorio.ObtenerListaSimple(id);
        }

        GestionMantenimientoDetalle IServicioBase<GestionMantenimientoDetalle>.Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        IEnumerable<GestionMantenimientoDetalle> IServicioBase<GestionMantenimientoDetalle>.ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }

        //public IEnumerable<Parametro> ObtenerTodos()
        //{
        //    return _repositorio.ObtenerLista();
        //}
    }
}
