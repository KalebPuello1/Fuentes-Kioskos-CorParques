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

    public class ServicioOrden : IServicioOrden
    {

        private readonly IRepositorioOrden _repositorio;

        #region Constructor

        public ServicioOrden(IRepositorioOrden repositorio)
        {
            _repositorio = repositorio;
        }


        #endregion

        #region Metodos

        public IEnumerable<Orden> ObtenerListaOrden()
        {
            return _repositorio.ObtenerListaOrdenes();
        }
        #endregion



    }
}
