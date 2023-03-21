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
    public class ServicioAlistamientoPendiente : IServicioAlistamientoPendiente
    {
        private readonly IRepositorioAlistamientoPendiente _respositorio;

        public  ServicioAlistamientoPendiente(IRepositorioAlistamientoPendiente repositorio)
        {
            _respositorio = repositorio;
        }

        public IEnumerable<AlistamientoPendiente> ObtenerAlistamientosPendientes(int IdReporte)
        {
            return _respositorio.ObtenerAlistamientosPendientes(IdReporte);
        }
    }
}
