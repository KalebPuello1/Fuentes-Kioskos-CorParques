using Corparques.Datos.Contratos;
using CorParques.Negocio.Contratos;
using CorParques.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorParques.Negocio.Nucleo
{
    public class ServicioAlmacen : IServicioAlmacen
    {
        public readonly IRepositorioAlmacen repositorio;

        public ServicioAlmacen(IRepositorioAlmacen repositori)
        {
            repositorio = repositori;
        }
        public IEnumerable<Almacen> getAllAlmacen()
        {
           return repositorio.getAllAlmacen();
        }
    }
}
