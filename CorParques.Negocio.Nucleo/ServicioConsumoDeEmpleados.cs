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
    public class ServicioConsumoDeEmpleados : IServicioConsumoDeEmpleados
    {
        #region properties
        private readonly IRepositorioConsumoDeEmpleado _repositorio;
        #endregion
        #region constructor
        public ServicioConsumoDeEmpleados(IRepositorioConsumoDeEmpleado repositorio)
        {
            _repositorio = repositorio;
        }

        #endregion

        public IEnumerable<ConsumoDeEmpleados>[] buscar(string FInicial, string FFinal, string NDocumento, string Area)
        {
            var _list = _repositorio.buscar(FInicial, FFinal, NDocumento, Area);
            return _list;
        }

        public IEnumerable<EstructuraEmpleado> buscarEmpresas()
        {
            var _list = _repositorio.buscarEmpresas();
            return _list;
        }

        public void tested()
        {
            Console.WriteLine("lolando");
        }
    }
}
