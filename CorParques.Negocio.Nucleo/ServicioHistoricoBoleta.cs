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
    public class ServicioHistoricoBoleta : IServicioHistoricoBoleta
    {

        private readonly IRepositorioHistoricoBoleta _repositorio;


        public ServicioHistoricoBoleta(IRepositorioHistoricoBoleta repositorio)
        {
            _repositorio = repositorio;
        }

        public DetalleBoleta ObtenerHistoricoBoleta(string consecutivo)
        {
            return _repositorio.ObtenerHistoricoBoleta(consecutivo);
        }
    }
}
