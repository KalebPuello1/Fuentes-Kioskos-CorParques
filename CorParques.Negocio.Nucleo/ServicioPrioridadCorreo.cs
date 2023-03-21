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
    public class ServicioPrioridadCorreo : IServicioPrioridadCorreo
    {
        IRepositorioPrioridadCorreo servicio;

        public ServicioPrioridadCorreo(IRepositorioPrioridadCorreo _servicio)
        {
            servicio = _servicio;
        }


        public IEnumerable<PrioridadCorreo> ObtenerTodos()
        {
            return servicio.ObtenerLista();
        }
    }
}
