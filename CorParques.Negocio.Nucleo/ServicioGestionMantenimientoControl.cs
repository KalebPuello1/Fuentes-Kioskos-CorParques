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
    public class ServicioGestionMantenimientoControl : IServicioGestionMantenimientoControl
    {
        private readonly IRepositorioGestionMantenimientoControl _repositorio;

        
        public ServicioGestionMantenimientoControl(IRepositorioGestionMantenimientoControl gestion)
        {
            _repositorio = gestion;
        }

        public bool Actualizar(GestionMantenimientoControl modelo)
        {
            throw new NotImplementedException();
        }

        public bool ActualizarMantenbimientoControl(GestionMantenimientoControl Modelo)
        {
            return _repositorio.ActualizarMantenbimientoControl(Modelo);
        }

        public GestionMantenimientoControl Crear(GestionMantenimientoControl modelo)
        {
            throw new NotImplementedException();
        }

        public GestionMantenimientoControl Obtener(int id)
        {
            return _repositorio.ObtenerxMantenimiento(id);
        }

        public IEnumerable<GestionMantenimientoControl> ObtenerTodos()
        {
            return _repositorio.ObtenerTodosMantenimientos();
        }

        //public IEnumerable<Parametro> ObtenerTodos()
        //{
        //    return _repositorio.ObtenerLista();
        //}
    }
}
