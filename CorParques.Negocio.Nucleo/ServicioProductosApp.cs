//Cambioquitar: Este controlador usa el enumerador de perfiles.
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
    public class ServicioProductosApp : IServicioProductosApp
    {
        IRepositorioProductosApp _repositorio;
        

        public ServicioProductosApp(IRepositorioProductosApp repositorio)
        {
            _repositorio = repositorio;
            
        }

        public bool Insertar(ref Producto producto)
        {
            return _repositorio.Insertar(producto);
        }

        public bool Crear(ref Producto producto)
        {
            
            return _repositorio.Crear(producto);
        }

        public bool Inactivar(int IdProducto,int activo)
        {
            return _repositorio.Inactivar(IdProducto,activo);
        }

        public IEnumerable<Producto> Obtener()
        {
            return _repositorio.Obtener();
        }

        public Producto ObtenerId(int IdProducto)
        {
           return _repositorio.ObtenerId(IdProducto);
        }
    }
}
