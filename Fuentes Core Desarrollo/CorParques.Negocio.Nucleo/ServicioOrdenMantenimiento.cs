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
    public class ServicioOrdenMantenimiento : IServicioOrdenMantenimiento
    {
        private readonly IRepositorioOrdenMantenimiento _repositorio;
        public ServicioOrdenMantenimiento(IRepositorioOrdenMantenimiento repositorio)
        {
            _repositorio = repositorio;
        }

        public bool ActualizaHoraOrden(string observaciones, int IdUsuarioAprobador, long NumeroOrden, int Aprobado, int IdOperaciones, int Procesado, string CodSapPunto)
        {
            return _repositorio.ActualizaHoraOrden(observaciones, IdUsuarioAprobador, NumeroOrden, Aprobado, IdOperaciones, Procesado, CodSapPunto);
        }

        public IEnumerable<OrdenMantenimiento> ObtenerOrdenesMantenimiento(string Punto, long NumeroOrden)
        {
            return _repositorio.ObtenerOrdenesMantenimiento(Punto, NumeroOrden);
        }
        
        public IEnumerable<Retorno> ObtenerRetornoPorNumeroOrden(long NumeroOrden)
        {
            var rta = _repositorio.ObtenerRetornoPorNumeroOrden(NumeroOrden);
            return rta;
        }
        
        /// <summary>
        /// RDSH: Retorna una orden de mantenimiento por el numero de la orden.
        /// </summary>
        /// <param name="intNumeroOrden"></param>
        /// <returns></returns>
        public OrdenMantenimiento ObtenerOrdenMantenimiento(long intNumeroOrden)
        {
            return _repositorio.ObtenerOrdenMantenimiento(intNumeroOrden);
        }

    }
}
