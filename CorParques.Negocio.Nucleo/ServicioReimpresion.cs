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
    public class ServicioReimpresion : IServicioReimpresion
    {
        private readonly IRepositorioReimpresion _repositorio;

        public ServicioReimpresion(IRepositorioReimpresion repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<Reimpresion> GetReimpresion(ReimpresionFiltros modelo)
        {
            return _repositorio.GetReimpresion(modelo);
        }

        public IEnumerable<Reimpresion> ObtenerReimpresion(string Punto, string FechaInicial, string FechaFinal, string HoraInicial, string HoraFinal, string CodBrazalete, string CodInicialFactura, string CodFinalFactura)
        {
            return _repositorio.ObtenerReimpresion(Punto, FechaInicial, FechaFinal, HoraInicial, HoraFinal, CodBrazalete, CodInicialFactura, CodFinalFactura);
        }
    }
}
