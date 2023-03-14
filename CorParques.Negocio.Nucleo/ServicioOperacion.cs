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
    public class ServicioOperacion : IServicioOperacion
    {
        private readonly IRepositorioOperacion _repositorio;
        public ServicioOperacion(IRepositorioOperacion repositorio)
        {
            _repositorio = repositorio;
        }
              
        public IEnumerable<Operaciones> ObtenerOrdenes(string Punto, string NumDocumento)
        {
            return _repositorio.ObtenerOrdenes(Punto, NumDocumento);
        }

        public Operaciones ObtenerOrdenPorNumeroOrden(long NumeroOrden)
        {
            return _repositorio.ObtenerOrdenPorNumeroOrden(NumeroOrden);
        }

        /// <summary>
        /// RDSH: Retorna las operaciones por numero de orden.
        /// </summary>
        /// <param name="intNumeroOrden"></param>
        /// <returns></returns>
        public IEnumerable<Operaciones> ObtenerOperacionesPorOrden(long intNumeroOrden)
        {
            return _repositorio.ObtenerOperacionesPorOrden(intNumeroOrden);
        }

    }
}
