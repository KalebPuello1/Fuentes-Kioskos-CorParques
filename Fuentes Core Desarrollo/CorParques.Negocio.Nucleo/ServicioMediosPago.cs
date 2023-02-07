using CorParques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioMediosPago : IServicioMediosPago
    {
        protected IRepositorioMediosPagos _repositorio;

        public ServicioMediosPago(IRepositorioMediosPagos repositorio)
        {
            this._repositorio = repositorio;
        }


        public bool Actualizar(MediosPago modelo)
        {
            return _repositorio.Actualizar(ref modelo);
        }

        public MediosPago Crear(MediosPago modelo)
        {
            modelo.FechaCreacion = DateTime.Now;
            modelo.IdEstado = 1;
            modelo.Id = _repositorio.Insertar(ref modelo);

            return modelo;
        }

        public MediosPago Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<TipoGeneral> ObtenerListaSimple()
        {
            return _repositorio.ListaSimple();
        }

        public IEnumerable<MediosPago> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }
    }
}
