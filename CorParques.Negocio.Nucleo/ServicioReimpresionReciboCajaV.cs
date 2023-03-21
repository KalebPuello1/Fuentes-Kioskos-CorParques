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
    public class ServicioReimpresionReciboCajaV : IServicioReimpresionReciboCajaV
    {
        private readonly IRepositorioReimpresionReciboCajaV repositorioReimpresion;
        public ServicioReimpresionReciboCajaV(IRepositorioReimpresionReciboCajaV repo)
        {
            repositorioReimpresion = repo;
        }
        public IEnumerable<ReimpresionReciboCajaV> datosReimpresion(string datoI, string datoF)
        {
            return repositorioReimpresion.datosReimpresion(datoI, datoF);
        }
        public ReimpresionReciboCajaV datoReimpresion()
        {
            return repositorioReimpresion.datoReimpresion();
        }



    }
}
