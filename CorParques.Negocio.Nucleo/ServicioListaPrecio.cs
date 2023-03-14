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
    public class ServicioListaPrecio : IServicioListaPrecio
    {
        IRepositorioListaPrecio _repositorio;

        public ServicioListaPrecio(IRepositorioListaPrecio repositorio)
        {
            _repositorio = repositorio;
        }

        public bool Actualizar(ListaPrecio lista)
        {
            return _repositorio.Actualizar(ref lista);
        }

        public bool Eliminar(ListaPrecio lista)
        {
            return _repositorio.Eliminar(lista);
        }

        public ListaPrecio Insertar(ListaPrecio lista)
        {
            if (_repositorio.Insertar(ref lista)>0)
                return lista;
            return null;
        }

        public ListaPrecio Obtener(int id)
        {
            return _repositorio.Obtener(id);
        }

        public IEnumerable<ListaPrecio> ObtenerTodos()
        {
            return _repositorio.ObtenerLista();
        }
    }
}
